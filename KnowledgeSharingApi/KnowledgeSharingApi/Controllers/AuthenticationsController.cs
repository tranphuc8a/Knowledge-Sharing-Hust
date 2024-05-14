using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.AuthenticationModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Infrastructures.Interfaces.OAuth2;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KnowledgeSharingApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationsController(
            IRegisterNewUserService registerNewUserService,
            IForgotPasswordService forgotPasswordService,
            IAuthenticationService authenticationService,
            IResourceFactory resourceFactory,
            IOAuth2Service OAuth2Service
        ) : BaseController
    {
        protected readonly IRegisterNewUserService registerNewUserService = registerNewUserService;
        protected readonly IOAuth2Service OAuth2Service = OAuth2Service;
        protected readonly IForgotPasswordService forgotPasswordService = forgotPasswordService;
        protected readonly IAuthenticationService authenticationService = authenticationService;
        protected readonly IResourceFactory resourceFactory = resourceFactory;
        protected readonly IResponseResource responseResource = resourceFactory.GetResponseResource();

        #region Register Account
        /// <summary>
        /// Gửi verify code tới email cần đăng ký tài khoản
        /// </summary>
        /// <param name="email"> email cần đăng ký tài khoản mới </param>
        /// <returns></returns>
        /// Created: PhucTV (13/3/24)
        /// Modified: None
        [HttpGet("send-register-account-verify-code")]
        public virtual async Task<IActionResult> SendRegisterAccountVerifyCode(string? email)
        {
            ServiceResult res = await registerNewUserService.SendVerifyCode(email);
            return StatusCode((int) res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Kiểm tra verify code
        /// </summary>
        /// <param name="codeModel"> thông tin mã xác minh cần kiểm tra </param>
        /// <returns></returns>
        /// Created: PhucTV (13/3/24)
        /// Modified: None
        [HttpPost("check-register-account-verify-code")]
        public virtual async Task<IActionResult> CheckRegisterAccountVerifyCode(VerifyCodeModel codeModel)
        {
            ServiceResult res = await registerNewUserService.CheckVerifyCode(codeModel);
            return StatusCode((int) res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        /// <param name="codeModel"> thông tin mã xác minh cần kiểm tra </param>
        /// <returns></returns>
        /// Created: PhucTV (13/3/24)
        /// Modified: None
        [HttpPost("register-account")]
        public virtual async Task<IActionResult> RegisterAccount(ActiveCodeRegisterModel codeModel)
        {
            ServiceResult res = await registerNewUserService.Action(codeModel);
            return StatusCode((int) res.StatusCode, new ApiResponse(res));
        }
        #endregion

        #region Reset forgot password
        /// <summary>
        /// Gửi verify code tới email cần reset mật khẩu
        /// </summary>
        /// <param name="email"> email cần reset mật khẩu </param>
        /// <returns></returns>
        /// Created: PhucTV (13/3/24)
        /// Modified: None
        [HttpGet("send-reset-password-verify-code")]
        public virtual async Task<IActionResult> SendResetPasswordVerifyCode(string? email)
        {
            ServiceResult res = await forgotPasswordService.SendVerifyCode(email);
            return StatusCode((int) res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Kiểm tra verify code
        /// </summary>
        /// <param name="codeModel"> thông tin mã xác minh cần kiểm tra </param>
        /// <returns></returns>
        /// Created: PhucTV (13/3/24)
        /// Modified: None
        [HttpPost("check-reset-password-verify-code")]
        public virtual async Task<IActionResult> CheckResetPasswordVerifyCode(VerifyCodeModel codeModel)
        {
            ServiceResult res = await forgotPasswordService.CheckVerifyCode(codeModel);
            return StatusCode((int) res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Reset mật khẩu mới
        /// </summary>
        /// <param name="codeModel"> thông tin mật khẩu mới cần thay đổi </param>
        /// <returns></returns>
        /// Created: PhucTV (13/3/24)
        /// Modified: None
        [HttpPost("reset-password")]
        public virtual async Task<IActionResult> ResetPassword(ActiveCodeForgotPasswordModel codeModel)
        {
            ServiceResult res = await forgotPasswordService.Action(codeModel);
            return StatusCode((int) res.StatusCode, new ApiResponse(res));
        }
        #endregion


        #region Google OAuth2

        /// <summary>
        /// Xử lý yêu cầu đăng nhập bang google token
        /// </summary>
        /// <param name="token"> google token </param>
        /// <returns></returns>
        /// Created: PhucTV (10/4/24)
        /// Modified: None
        [HttpPost]
        [Route("login/google")]
        public async Task<IActionResult> GoogleLogin(string token)
        {
            ServiceResult res = await OAuth2Service.LoginByGoogle(token);
            return StatusCode((int) res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xử lý yêu cầu lay ma dang ky tai khoan moi bang google token
        /// </summary>
        /// <param name="token"> google token </param>
        /// <returns></returns>
        /// Created: PhucTV (10/4/24)
        /// Modified: None
        [HttpPost]
        [Route("register/google")]
        public async Task<IActionResult> GetRegisterActiveCodeByGoogleToken(string token)
        {
            ServiceResult res = await OAuth2Service.RequestSignupByGoogle(token);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu tao moi tai khoan bang active code
        /// </summary>
        /// <param name="model"> Thong tin tai khoan tao moi </param>
        /// <returns></returns>
        /// Created: PhucTV (10/4/24)
        /// Modified: None
        [HttpPost]
        [Route("create-new-user")]
        public async Task<IActionResult> CreateNewUser(ActiveCodeRegisterModel model)
        {
            ServiceResult res = await OAuth2Service.RegsiterUserByActiveCode(model);
            return StatusCode(res);
        }

        #endregion


        /// <summary>
        /// Xử lý yêu cầu đăng nhập tài khoản
        /// </summary>
        /// <param name="model"> username và password đăng nhập </param>
        /// <returns></returns>
        /// Created: PhucTV (22/2/24)
        /// Modified: None
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            ServiceResult res = await authenticationService.Login(model);
            return StatusCode((int) res.StatusCode, new ApiResponse(res));
        }

        /// <summary>
        /// Xử lý yêu cầu đăng nhập tài khoản bằng captcha
        /// </summary>
        /// <param name="model"> username và password đăng nhập </param>
        /// <returns></returns>
        /// Created: PhucTV (01/03/24)
        /// Modified: None
        [HttpPost]
        [Route("login-with-captcha")]
        public async Task<IActionResult> Login([FromBody] LoginWithCaptchaModel model)
        {
            ServiceResult res = await authenticationService.LoginWithCaptcha(model);
            return StatusCode((int) res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Xử lý yêu cầu tạo mới captcha
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (01/03/24)
        /// Modified: None
        [HttpGet]
        [Route("refresh-captcha")]
        public async Task<IActionResult> RefreshCaptcha()
        {
            ServiceResult res = await authenticationService.GenerateNewCaptcha();
            return StatusCode((int) res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Xử lý yêu cầu thay đổi mật khẩu
        /// </summary>
        /// <param name="changePasswordModel"> thông tin mặt khẩu mới </param>
        /// <returns></returns>
        /// Created: PhucTV (22/2/24)
        /// Modified: None
        [CustomAuthorization(Roles: "User, Admin, Banned")]
        [HttpPost]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel changePasswordModel)
        {
            string? username = HttpContext.User.Identity?.Name;
            ServiceResult res;
            if (username != changePasswordModel.Username)
            {
                res = ServiceResult.BadRequest(responseResource.InvalidUsername());
            }
            else
            {
                res = await authenticationService.ChangePassword(changePasswordModel);
            }
            return StatusCode((int) res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Xử lý yêu cầu làm mới token
        /// </summary>
        /// <param name="tokenModel"> accessToken và refresh token cũ </param>
        /// <returns></returns>
        /// Created: PhucTV (22/2/24)
        /// Modified: None
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            ServiceResult res = await authenticationService.RefreshToken(tokenModel);
            return StatusCode((int) res.StatusCode, new ApiResponse(res));
        }


        /// <summary>
        /// Đăng xuất khỏi phiên đăng nhập hiện tại
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (22/2/24)
        /// Modified: None
        [CustomAuthorization]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            string sessionId = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.Sid) ?? string.Empty;
            ServiceResult res = await authenticationService.Logout(Guid.Parse(sessionId));
            return StatusCode((int) res.StatusCode, new ApiResponse(res));
        }



        /// <summary>
        /// Đăng xuất khỏi toàn bộ phiên đăng nhập
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (22/2/24)
        /// Modified: None
        [CustomAuthorization]
        [HttpPost]
        [Route("logout-all")]
        public async Task<IActionResult> LogoutAll ()
        {
            string username = HttpContext.User.Identity?.Name!;
            ServiceResult res = await authenticationService.LogoutAll(username);
            return StatusCode((int) res.StatusCode, new ApiResponse(res));
        }
    }
} 
