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
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories;
using KnowledgeSharingApi.Services.Interfaces.Authentications;
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

namespace KnowledgeSharingApi.Services.Services.Authentications
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

        #region Functional methods

        /// <summary>
        /// Hàm sinh ra một mã refresh token ngẫu nhiên
        /// </summary>
        /// <returns> string - mã token sinh được </returns>
        /// Created: PhucTV (23/2/24)
        /// Modified: None
        protected virtual string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[64];
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
                DateTime now = DateTime.UtcNow;
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
                    return ServiceResult.ServerError(ResponseResource.ServerError(), string.Empty, new
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
            if (user == null) return ServiceResult.BadRequest("Bạn chưa đăng ký tài khoản bằng email này, hãy thực hiện đăng ký tài khoản trước");

            // return Dang nhap thanh cong
            return await LoginSuccess(user!);
        }

        public async Task<ServiceResult> RegsiterUserByActiveCode(ActiveCodeRegisterModel model)
        {
            // Check token
            CheckActiveCode(model.ActiveCode!, out string? emailDecoded, out string? name, out string? picture);
            if (emailDecoded == null)
                return ServiceResult.UnAuthorized(ResponseResource.InvalidToken());

            // check email is match:
            if (emailDecoded != model.Email!)
                return ServiceResult.BadRequest("Email không khớp với ActiveCode");

            // Check email va username chua ton tai
            User? userByEmail = await UserRepository.GetByEmail(model.Email!);
            if (userByEmail != null) return ServiceResult.BadRequest(ResponseResource.ExistedUser());
            User? userByUsername = await UserRepository.GetByUsername(model.Username!);
            if (userByUsername != null) return ServiceResult.BadRequest("Username đã tồn tại");

            // Tao moi user, profile, role
            User user = new()
            {
                CreatedBy = model.Username!,
                CreatedTime = DateTime.UtcNow,
                UserId = Guid.NewGuid(),
                Email = model.Email!,
                Username = model.Username!,
                Role = UserRoles.User
            };
            Guid? id = await UserRepository.RegisterUser(user.UserId, user, model.Password!, name ?? model.Username!, picture);
            if (id == null) return ServiceResult.ServerError(ResponseResource.ServerError());

            // Tra ve thanh cong
            return ServiceResult.Success(ResponseResource.Success(), string.Empty, user);
        }

        /// <summary>
        /// Decode active code va tra ve email duoc ma hoa
        /// </summary>
        /// <param name="activeCode"> token can check </param>
        /// <returns> email hoac null </returns>
        /// Created: PhucTV (10/4/24)
        /// Modified: None
        protected virtual void CheckActiveCode(string activeCode, out string? email, out string? name, out string? picture)
        {
            email = null;
            name = null;
            picture = null;

            List<Claim>? claims = Encrypt.JwtDecryptToListClaims(activeCode, isValidateLifeTime: true)?.ToList();
            if (claims == null) return;
            string? roleType = claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault()?.Value;
            if (roleType != RoleType) return;

            email = claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault()?.Value;
            name = claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
            picture = claims.Where(c => c.Type == ClaimTypes.Thumbprint).FirstOrDefault()?.Value;
        }

        public async Task<ServiceResult> RequestSignupByGoogle(string googleToken)
        {
            // Check token hop le
            GoogleOAuth2User? userInfor = await GoogleOAuth2.GetuserByToken(googleToken);
            if (userInfor == null) return ServiceResult.UnAuthorized(ResponseResource.InvalidToken());

            // Kiem tra chua ton tai trong db
            User? user = await UserRepository.GetByEmail(userInfor.email);
            if (user != null) return ServiceResult.BadRequest("Bạn đã đăng ký tài khoản bằng email này rồi, hãy chọn đăng nhập");

            // Sinh Active code va tra ve
            List<Claim> claims = [
                new(ClaimTypes.Role, RoleType),
                new(ClaimTypes.Email, userInfor.email),
                new(ClaimTypes.Name, userInfor.name),
                new(ClaimTypes.Thumbprint, userInfor.picture ?? string.Empty)
            ];
            string? token = Encrypt.JwtEncrypt(claims);
            if (token == null) return ServiceResult.ServerError(ResponseResource.ServerError());
            return ServiceResult.Success(ResponseResource.Success(), string.Empty, new
            {
                ActiveCode = token
            });
        }
    }
}
