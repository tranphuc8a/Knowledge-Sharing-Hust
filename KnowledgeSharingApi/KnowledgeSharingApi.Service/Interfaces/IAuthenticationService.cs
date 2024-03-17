using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Xử lý dịch vụ đăng nhập
        /// </summary>
        /// <param name="userLogin"> Thông tin tài khoản mật khẩu đăng nhập  </param>
        /// <returns></returns>
        /// Created: PhucTV (21/2/24)
        /// Modified: None
        Task<ServiceResult> Login(LoginModel userLogin);


        /// <summary>
        /// Xử lý dịch vụ đăng nhập với captcha
        /// </summary>
        /// <param name="userLogin"> Thông tin đăng nhập với captcha  </param>
        /// <returns></returns>
        /// Created: PhucTV (01/03/24)
        /// Modified: None
        Task<ServiceResult> LoginWithCaptcha(LoginWithCaptchaModel userLogin);


        /// <summary>
        /// Dịch vụ tạo một mã captcha mới
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (01/03/2024)
        /// Modified: None
        Task<ServiceResult> GenerateNewCaptcha();


        /// <summary>
        /// Xử lý dịch vụ đăng xuất phiên đăng nhập hiện tại
        /// </summary>
        /// <param name="sessionId"> sessionId cần đăng xuất </param>
        /// <returns></returns>
        /// Created: PhucTV (21/2/24)
        /// Modified: None
        Task<ServiceResult> Logout(string sessionId);

        
        /// <summary>
        /// Xử lý dịch vụ đăng xuất tất cả phiên đăng nhập của user
        /// </summary>
        /// <param name="username"> username cần đăng xuất </param>
        /// <returns></returns>
        /// Created: PhucTV (21/2/24)
        /// Modified: None
        Task<ServiceResult> LogoutAll(string username);


        /// <summary>
        /// Dịch vụ Cấp mới Access Token và Refresh Token 
        /// </summary>
        /// <param name="tokenModel"> token cũ cần cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (21/2/24)
        /// Modified: None
        Task<ServiceResult> RefreshToken(TokenModel tokenModel);


        /// <summary>
        /// Xử lý dịch vụ yêu cầu thay đổi mật khẩu
        /// </summary>
        /// <param name="changePasswordModel"> Đối tượng chứa thông tin thay đổi mật khẩu </param>
        /// <returns></returns>
        /// Created: PhucTV (21/2/24)
        /// Modified: None
        Task<ServiceResult> ChangePassword(ChangePasswordModel changePasswordModel);
    }
}
