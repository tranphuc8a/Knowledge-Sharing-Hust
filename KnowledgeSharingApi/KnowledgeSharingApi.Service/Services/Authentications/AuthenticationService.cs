using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.AuthenticationModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Infrastructures.Interfaces.Caches;
using KnowledgeSharingApi.Infrastructures.Interfaces.Captcha;
using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories;
using KnowledgeSharingApi.Services.Interfaces.Authentications;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services.Authentications
{
    public class AuthenticationService(
            IConfiguration configuration,
            IEncrypt encrypt,
            ICache cache,
            ICaptcha captcha,
            IResourceFactory resourceFactory,
            IUserRepository userRepository,
            ISessionRepository sessionRepository
        ) : IAuthenticationService
    {
        protected readonly ICache cache = cache;
        protected readonly IConfiguration configuration = configuration;
        protected readonly IEncrypt _encrypt = encrypt;
        protected readonly IUserRepository userRepository = userRepository;
        protected readonly ISessionRepository sessionRepository = sessionRepository;
        protected readonly IResponseResource responseResource = resourceFactory.GetResponseResource();
        protected readonly ICaptcha Captcha = captcha;
        protected int TimeThresholdOnMinutes = 2;
        protected int MaxFailedLoginAttempts = 5;
        protected Random random = new();


        #region Redefine Access Cache Avoid Duplicate key
        protected virtual string BoundKey(string key)
        {
            return $"Set Check Login Attack {key}";
        }
        protected virtual void CacheSet(CheckLoginAttackCacheDto value)
        {
            cache.Set(BoundKey(value.Username), value);
        }
        protected virtual CheckLoginAttackCacheDto? CacheGet(string username)
        {
            return cache.Get<CheckLoginAttackCacheDto>(BoundKey(username));
        }
        #endregion


        public virtual Task<ServiceResult> GenerateNewCaptcha()
        {
            string captcha = Captcha.GenerateRandomCaptcha(6);
            string? captchaToken = Captcha.EncodeCaptcha(captcha);
            byte[] imageData = Captcha.GenerateBase64ImageCaptcha(captcha);
            return Task.FromResult(ServiceResult.Success(
                responseResource.CaptchaCreated(),
                string.Empty, new
                {
                    Token = captchaToken,
                    ImageData = imageData
                }
            ));
        }

        protected virtual async Task CheckLoginAttack(string username)
        {
            CheckLoginAttackCacheDto? cacheModel = CacheGet(username);
            if (cacheModel == null)
            {
                // Lần đầu đăng nhập, khởi tạo Cache và lưu lại, cho pass
                cacheModel = new CheckLoginAttackCacheDto()
                {
                    Username = username,
                    LastAccessTime = DateTime.UtcNow,
                    NumberFailedAttempt = 0
                };
                CacheSet(cacheModel);
            }
            else
            {
                // Kiểm tra Yêu cầu đăng nhập sai quá nhiều lần trong khoảng thời gian ngắn hơn Threshold
                bool isFasterThanTimeThreshold =
                     cacheModel.LastAccessTime.AddMinutes(TimeThresholdOnMinutes) >= DateTime.UtcNow;
                bool isOverMaxFailedAccess = cacheModel.NumberFailedAttempt >= MaxFailedLoginAttempts;

                // Cập nhật lại lần đăng nhập cuối
                cacheModel.LastAccessTime = DateTime.UtcNow;
                CacheSet(cacheModel);

                if (isFasterThanTimeThreshold && isOverMaxFailedAccess)
                {
                    // Đã phát hiện tấn công, yêu cầu đăng nhập bằng captcha
                    ServiceResult newCaptcha = await GenerateNewCaptcha();
                    string message = responseResource.LimitLoginTime();
                    throw new ResponseException()
                    {
                        StatusCode = EStatusCode.BadRequest,
                        UserMessage = message,
                        DevMessage = message,
                        Body = newCaptcha.Data
                    };
                }
            }
        }

        public virtual async Task<ServiceResult> Login(LoginModel userLogin)
        {
            ServiceResult loginFailed = new()
            {
                IsSuccess = false,
                StatusCode = EStatusCode.Unauthorized,
                UserMessage = responseResource.LoginFailure(),
                DevMessage = responseResource.LoginFailure()
            };

            // Step 1. Kiểm tra username phải tồn tại trong hệ thống
            User? user = await userRepository.GetByUsername(userLogin.Username!);
            if (user == null) return loginFailed;

            // Step 2. Kiểm tra Check Login Attack
            await CheckLoginAttack(user.Username);


            // Step 3. Kiểm tra mật khẩu hợp lệ
            bool isPasswordValid = await userRepository.CheckPassword(user.Username, userLogin.Password!, user.HashPassword);
            if (!isPasswordValid)
            {
                // Cập nhật lại Check login model trong Cache, đảm bảo đã có trong Cache
                CheckLoginAttackCacheDto checkLogin = CacheGet(user.Username)!;
                checkLogin.NumberFailedAttempt++;
                checkLogin.LastAccessTime = DateTime.UtcNow;
                CacheSet(checkLogin);

                return loginFailed;
            }

            // Step 4. Đăng nhập thành công:
            return await LoginSuccess(user!);
        }

        public virtual async Task<ServiceResult> LoginWithCaptcha(LoginWithCaptchaModel userLogin)
        {
            ServiceResult newCaptcha = await GenerateNewCaptcha();
            ServiceResult wrongCaptcha = new()
            {
                IsSuccess = false,
                StatusCode = EStatusCode.Unauthorized,
                UserMessage = responseResource.InvalidCaptcha(),
                DevMessage = responseResource.InvalidCaptcha(),
                Data = newCaptcha.Data
            };
            ServiceResult loginFailed = new()
            {
                IsSuccess = false,
                StatusCode = EStatusCode.Unauthorized,
                UserMessage = responseResource.LoginFailure(),
                DevMessage = responseResource.LoginFailure(),
                Data = newCaptcha.Data
            };

            // Step 1. Decode token hợp lệ với captcha
            string? captcha = Captcha.DecodeCaptcha(userLogin.Token!);
            if (captcha == null || userLogin.Captcha != captcha)
                return wrongCaptcha;

            // Step 2. Kiểm tra username hợp lệ,
            User? user = await userRepository.GetByUsername(userLogin.Username!);
            if (user == null) return loginFailed;

            // Step 3. Cập nhật lần cuối đăng nhập
            CheckLoginAttackCacheDto checkLogin = CacheGet(user.Username)!;
            checkLogin.LastAccessTime = DateTime.UtcNow;
            CacheSet(checkLogin);

            // Step 4. Kiểm tra password hợp lệ
            bool checkPassword = await userRepository.CheckPassword(user.Username, userLogin.Password!);
            if (!checkPassword) return loginFailed;

            // Step 5. Đăng nhập thành công
            return await LoginSuccess(user!);
        }

        public virtual async Task<ServiceResult> Logout(Guid sessionId)
        {
            int rowEffect = await sessionRepository.Delete(sessionId);

            if (rowEffect <= 0)
                return ServiceResult.ServerError(responseResource.LogoutFailure());

            return ServiceResult.Success(responseResource.LogoutSuccess());
        }

        public virtual async Task<ServiceResult> LogoutAll(string username)
        {
            int rowEffect = await sessionRepository.DeleteByUsername(username);

            if (rowEffect <= 0)
                return ServiceResult.ServerError(responseResource.LogoutFailure());

            return ServiceResult.Success(responseResource.LogoutSuccess());
        }

        public virtual async Task<ServiceResult> RefreshToken(TokenModel tokenModel)
        {
            ServiceResult invalidToken = ServiceResult.BadRequest(responseResource.InvalidToken());

            // Step 1. Decode access token để lấy về username đăng nhập
            JwtTokenDto? jwtTokenDto = _encrypt.JwtDecrypt(tokenModel.AccessToken!, false);
            if (jwtTokenDto == null) return invalidToken;

            // Step 2. Kiểm tra Refresh token khớp và còn thời hạn
            Session? session = await sessionRepository.Get(jwtTokenDto.SessionId);
            if (session == null || session.RefreshToken != tokenModel.RefreshToken || session.Expired <= DateTime.UtcNow)
                return invalidToken;

            // Refresh token hợp lệ, sinh refresh token mới
            string? token = _encrypt.JwtEncrypt(jwtTokenDto);
            string newRefreshToken = GenerateRefreshToken();

            if (int.TryParse(configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays))
            {
                // Cập nhật refresh token vào db
                session.RefreshToken = newRefreshToken;
                session.Expired = DateTime.UtcNow.AddDays(refreshTokenValidityInDays);
                session.ModifiedTime = DateTime.UtcNow;
                session.ModifiedBy = jwtTokenDto.Username;
                int rows = await sessionRepository.Update(session.SessionId, session);

                // Lỗi không thêm được vào db
                if (rows <= 0)
                {
                    return ServiceResult.ServerError(responseResource.ServerError(), string.Empty, new
                    {
                        Token = token,
                        RefreshToken = newRefreshToken
                    });
                }

                // Trả về thành công
                return ServiceResult.Success(
                    responseResource.LoginSuccess(),
                    string.Empty,
                    new
                    {
                        Token = token,
                        RefreshToken = newRefreshToken,
                    }
                );
            }
            return ServiceResult.ServerError(responseResource.ServerError());
        }


        public virtual async Task<ServiceResult> ChangePassword(ChangePasswordModel model)
        {
            // Step 1. Kiểm tra username phải tồn tại
            User? user = await userRepository.GetByUsername(model.Username!);
            if (user == null) return ServiceResult.BadRequest(responseResource.NotExistUser());

            // Step 2. Kiểm tra password cũ đúng
            bool checkPassword = await userRepository.CheckPassword(user.Username, model.Password!);
            if (!checkPassword)
            {
                return ServiceResult.BadRequest(responseResource.WrongOldPassword());
            }

            // Step 3. Kiểm tra password mới hợp lệ (không trùng)
            if (model.NewPassword == model.Password)
            {
                return ServiceResult.BadRequest(responseResource.NewPasswordSameOldPassword());
            }

            // Step 4. OK. Cập nhật mật khẩu, trả về thành công
            int res = await userRepository.UpdatePassword(user.Username, model.NewPassword!);
            if (res <= 0)
            {
                return ServiceResult.ServerError(responseResource.ChangePasswordFailure(), string.Empty);
            }

            return ServiceResult.Success(responseResource.ChangePasswordSuccess());
        }


        #region Functional
        /// <summary>
        /// Xử lý đăng nhập thành công sau khi validate LoginModel
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
            string? token = _encrypt.JwtEncrypt(new JwtTokenDto()
            {
                Username = user.Username,
                UserId = user.UserId,
                Role = user.Role,
                SessionId = sessionId
            });
            string refreshToken = GenerateRefreshToken();

            // Cập nhật Refresh Token vào DB
            if (int.TryParse(configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays))
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
                Guid? newId = await sessionRepository.Insert(sessionId, session);

                // Lỗi
                if (newId == null)
                {
                    return ServiceResult.ServerError(responseResource.ServerError(), string.Empty, new
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
                    UserMessage = responseResource.LoginSuccess(),
                    Data = new
                    {
                        Token = token,
                        RefreshToken = refreshToken,
                    }
                };
            }
            return ServiceResult.ServerError(responseResource.ServerError());
        }


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
        #endregion
    }
}
