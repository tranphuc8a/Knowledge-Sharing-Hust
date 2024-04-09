using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.AuthenticationModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.OAuth2;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using KnowledgeSharingApi.Infrastructures.Interfaces.Caches;
using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;
using KnowledgeSharingApi.Infrastructures.Interfaces.OAuth2;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Cms.Ecc;
using Pomelo.EntityFrameworkCore.MySql.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class OAuth2Service : IOAuth2Service
    {
        protected readonly IUserRepository UserRepository;
        protected readonly IResourceFactory ResourceFactory;
        protected readonly IResponseResource ResponseResource;
        protected readonly IEntityResource EntityResource;
        protected readonly ISessionRepository SessionRepository;

        protected readonly IGoogleOAuth2 GoogleOAuth2;
        protected readonly ICache Cache;
        protected readonly IEncrypt Encrypt;
        protected readonly IConfiguration Configuration;

        protected readonly string UserResource, RoleType = "Register User By ActiveCode";

        public OAuth2Service(
            IUserRepository userRepository,
            ISessionRepository sessionRepository,
            IResourceFactory resourceFactory,
            IGoogleOAuth2 googleOAuth2,
            ICache cache,
            IEncrypt encrypt,
            IConfiguration configuration
        )
        {
            UserRepository = userRepository;
            SessionRepository = sessionRepository;
            ResourceFactory = resourceFactory;
            ResponseResource = ResourceFactory.GetResponseResource();
            EntityResource = ResourceFactory.GetEntityResource();

            GoogleOAuth2 = googleOAuth2;
            Cache = cache;
            Encrypt = encrypt;
            Configuration = configuration;

            UserResource = EntityResource.User();
        }

        #region Redefine Access Cache Avoid Duplicate key
        protected virtual string BoundKey(string key)
        {
            return $"Set Check Login Attack {key}";
        }
        protected virtual void CacheSet(CheckLoginAttackCacheDto value)
        {
            Cache.Set<CheckLoginAttackCacheDto>(BoundKey(value.Username), value);
        }
        protected virtual CheckLoginAttackCacheDto? CacheGet(string username)
        {
            return Cache.Get<CheckLoginAttackCacheDto>(BoundKey(username));
        }
        #endregion

        #region Functional methods

        /// <summary>
        /// Hàm sinh ra một mã refresh token ngẫu nhiên
        /// </summary>
        /// <returns> string - mã token sinh được </returns>
        /// Created: PhucTV (23/2/24)
        /// Modified: None
        protected virtual string GenerateRefreshToken()
        {
            Byte[] randomNumber = new byte[64];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Xử lý đăng nhập thành công
        /// </summary>
        /// <param name="user"> User đã đăng nhập thành công </param>
        /// <returns> Service Result</returns>
        /// Created: PhucTV (01/03/24)
        /// Modified: None
        protected virtual async Task<ServiceResult> LoginSuccess(User user)
        {
            // Cập nhật lại số lần đăng nhập lỗi = 0
            CheckLoginAttackCacheDto loginModel = CacheGet(user.Username)!;
            loginModel.NumberFailedAttempt = 0;
            CacheSet(loginModel);

            // Sinh token
            Guid sessionId = Guid.NewGuid();
            string? token = Encrypt.JwtEncrypt(new JwtTokenDto()
            {
                Username = user.Username,
                UserId = user.UserId,
                Role = user.Role,
                SessionId = sessionId
            });
            string refreshToken = GenerateRefreshToken();

            // Cập nhật Refresh Token vào DB
            if (int.TryParse(Configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays))
            {
                // Insert new Session
                DateTime now = DateTime.Now;
                Session session = new()
                {
                    SessionId = sessionId,
                    UserId = user.UserId,
                    RefreshToken = refreshToken,
                    Expired = now.AddDays(refreshTokenValidityInDays),
                    Time = now,
                    // Place, Device
                    CreatedBy = user.Username,
                    CreatedTime = now
                };
                Guid? newId = await SessionRepository.Insert(sessionId, session);

                // Lỗi
                if (newId == null)
                {
                    return ServiceResult.ServerError(ResponseResource.ServerError(), String.Empty, new
                    {
                        Token = token,
                        RefreshToken = refreshToken
                    });
                }

                // Thành công
                return new ServiceResult
                {
                    IsSuccess = true,
                    StatusCode = EStatusCode.Success,
                    UserMessage = ResponseResource.LoginSuccess(),
                    Data = new
                    {
                        Token = token,
                        RefreshToken = refreshToken,
                    }
                };
            }
            return ServiceResult.ServerError(ResponseResource.ServerError());
        }

        #endregion

        public async Task<ServiceResult> LoginByGoogle(string googleToken)
        {
            // Check token hop le bang cach lay ve user tu api cua google
            GoogleOAuth2User? userInfor = await GoogleOAuth2.GetuserByToken(googleToken);
            if (userInfor == null) return ServiceResult.UnAuthorized(ResponseResource.InvalidToken());

            // Kiem tra email ton tai trong db
            User? user = await UserRepository.GetByEmail(userInfor.email);
            if (user == null) return ServiceResult.BadRequest("Ban chua dang ky tai khoan, hay dang ky truoc");

            // return Dang nhap thanh cong
            return await LoginSuccess(user!);
        }

        public async Task<ServiceResult> RegsiterUserByActiveCode(ActiveCodeRegisterModel model)
        {
            // Check token
            bool isValidateToken = CheckActiveCode(model.ActiveCode!, model.Email!);
            if (!isValidateToken)
                return ServiceResult.UnAuthorized(ResponseResource.InvalidToken());

            // Check email va username chua ton tai
            User? userByEmail = await UserRepository.GetByEmail(model.Email!);
            if (userByEmail != null) return ServiceResult.BadRequest(ResponseResource.ExistedUser());
            User? userByUsername = await UserRepository.GetByUsername(model.Username!);
            if (userByUsername != null) return ServiceResult.BadRequest("Username da ton tai");

            // Tao moi user, profile, role
            User user = new()
            {
                CreatedBy = model.FullName,
                CreatedTime = DateTime.Now,
                UserId = Guid.NewGuid(),
                Email = model.Email!,
                Username = model.Username!,
                Role = UserRoles.User
            };
            Guid? id = await UserRepository.RegisterUser(user.UserId, user, model.Password!, model.FullName!);
            if (id == null) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Tra ve thanh cong
            return ServiceResult.Success(ResponseResource.Success(), string.Empty, user);
        }

        protected virtual bool CheckActiveCode(string activeCode, string email)
        {
            List<Claim>? claims = Encrypt.JwtDecryptToListClaims(activeCode, isValidateLifeTime: true)?.ToList();
            if (claims == null) return false;
            string? roleType = claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault()?.Value;
            if (roleType != RoleType) return false;
            string? emailToCheck = claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault()?.Value;
            if (emailToCheck != email) return false;
            return true;
        }

        public async Task<ServiceResult> RequestSigninByGoogle(string googleToken)
        {
            // Check token hop le
            GoogleOAuth2User? userInfor = await GoogleOAuth2.GetuserByToken(googleToken);
            if (userInfor == null) return ServiceResult.UnAuthorized(ResponseResource.InvalidToken());

            // Kiem tra chua ton tai trong db
            User? user = await UserRepository.GetByEmail(userInfor.email);
            if (user != null) return ServiceResult.BadRequest("Ban da dang ky tai khoan roi, hay dang nhap");

            // Sinh Active code va tra ve
            List<Claim> claims = [
                new(ClaimTypes.Role, RoleType),
                new(ClaimTypes.Email, user!.Email)
            ];
            string? token = Encrypt.JwtEncrypt(claims);
            if (token == null) return ServiceResult.ServerError(ResponseResource.ServerError());
            return ServiceResult.Success(ResponseResource.Success(), string.Empty, token);
        }
    }
}
