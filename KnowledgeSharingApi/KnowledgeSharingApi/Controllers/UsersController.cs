using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces;
using KnowledgeSharingApi.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace KnowledgeSharingApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [CustomAuthorization]
    public class UsersController : ControllerBase
    {
        #region Fields and Constructor
        protected readonly IUserService UserService;
        protected readonly IResourceFactory ResourceFactory;
        protected readonly IResponseResource ResponseResource;

        public UsersController(
            IUserService customerGroupService,
            IResourceFactory resourceFactory
            )
        {
            UserService = customerGroupService;
            ResourceFactory = resourceFactory;
            ResponseResource = ResourceFactory.GetResponseResource();
        }


        /// <summary>
        /// Xử lý yêu cầu lấy về profile của chính mình
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpGet("me")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> GetMeProfile()
        {
            string? uId = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier);
            if (uId == null) return FailedAuthentication(HttpContext.User);
            ServiceResult service = await UserService.GetMyUserProfile(Guid.Parse(uId));
            ApiResponse res = new(service);
            return StatusCode((int) res.StatusCode, res);
        }

        /// <summary>
        /// Xử lý yêu cầu cập nhật ảnh đại diện
        /// </summary
        /// <param name="avatar"> IFormFile - file ảnh đại điện được upload lên </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpPatch("me/update-avatar")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> UpdateMeProfile([FromBody] IFormFile avatar)
        {
            string? userId = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier);
            if (userId == null) return FailedAuthentication(HttpContext.User);
            ServiceResult service = await UserService.UpdateMyAvatarImage(Guid.Parse(userId), avatar);
            ApiResponse res = new(service);
            return StatusCode((int) res.StatusCode, res);
        }

        /// <summary>
        /// Xử lý yêu cầu cập nhật ảnh bìa
        /// </summary>
        /// <param name="cover"> IFormFile - nội dung file ảnh bìa </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpPatch("me/update-cover")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> UpdateMeAvatar([FromBody] IFormFile cover)
        {
            string? userId = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier);
            if (userId == null) return FailedAuthentication(HttpContext.User);
            ServiceResult service = await UserService.UpdateMyCoverImage(Guid.Parse(userId), cover);
            ApiResponse res = new(service);
            return StatusCode((int) res.StatusCode, res);
        }

        /// <summary>
        /// Xử lý yêu cầu cập nhật thông tin cá nhân
        /// </summary>
        /// <param name="model"> Thông tin mới cần cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpPatch("me/update-profile")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> UpdateMeProfile([FromBody] UpdateProfileModel model)
        {
            string? userId = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier);
            if (userId == null) return FailedAuthentication(HttpContext.User);
            ServiceResult service = await UserService.UpdateMyUserProfile(Guid.Parse(userId), model);
            ApiResponse res = new(service);
            return StatusCode((int) res.StatusCode, res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về thông tin của người khác
        /// </summary>
        /// <param name="unOruid"> username hoặc userid của user cần lấy thông tin về </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpGet("user-detail")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> GetMeUserDetail(string unOruid)
        {
            string? userId = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier);
            if (userId == null) return FailedAuthentication(HttpContext.User);
            ServiceResult service = await UserService.GetUserDetail(Guid.Parse(userId), unOruid);
            ApiResponse res = new(service);
            return StatusCode((int) res.StatusCode, res);
        }


        /// <summary>
        /// Người dùng tìm kiếm trang cá nhân của người khác
        /// </summary>
        /// <param name="searchKey"> Từ khóa tìm kiếm </param>
        /// <param name="limit"> Thuộc tính phân trang - số lượng bản ghi cần lấy </param>
        /// <param name="offset"> Thuộc tính phân trang - độ lệch bản ghi </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpGet("search")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> SearchMe(string searchKey, int? limit, int? offset)
        {
            string? userId = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier);
            if (userId == null) return FailedAuthentication(HttpContext.User);
            ServiceResult service = await UserService.SearchUser(Guid.Parse(userId), searchKey, limit, offset);
            ApiResponse res = new(service);
            return StatusCode((int) res.StatusCode, res);
        }

        /// <summary>
        /// Hàm xử lý ngoại lệ xác thực user thất bại
        /// </summary>
        /// <param name="user"> Thông tin user sau khi giải mã jwt token </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        protected virtual IActionResult FailedAuthentication(ClaimsPrincipal user)
        {
            string msg = ResponseResource.Failure();
            ApiResponse res = new()
            {
                StatusCode = EStatusCode.Unauthorized,
                UserMessage = msg,
                DevMessage = msg,
                Body = user
            };
            return StatusCode((int)res.StatusCode, res);
        }

        #endregion


        #region Admin

        /// <summary>
        /// Xử lý yêu cầu cập nhật thông tin tài khoản của quản trị viên
        /// </summary>
        /// <param name="uid"> user id của tài khoản cần cập nhật thông tin </param>
        /// <param name="model"> thông tin cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpPatch("admin/update-user/{uid}")]
        [CustomAuthorization(Roles: "Admin")]
        public virtual async Task<IActionResult> AdminUpdateUser(Guid uid, [FromBody] UpdateUserModel model)
        {
            ServiceResult service = await UserService.AdminUpdateUserInfo(uid, model);
            ApiResponse res = new(service);
            return StatusCode((int) res.StatusCode, res);
        }

        /// <summary>
        /// Xử lý yêu cầu mở khóa một tài khoản 
        /// </summary>
        /// <param name="uid"> user id của tài khoản được mở khóa </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpPost("admin/unblock-user/{uid}")]
        [CustomAuthorization(Roles: "Admin")]
        public virtual async Task<IActionResult> AdminUnblockUser(Guid uid)
        {
            ServiceResult service = await UserService.AdminUnblockUser(uid);
            ApiResponse res = new(service);
            return StatusCode((int) res.StatusCode, res);
        }


        /// <summary>
        /// Xử lý yêu cầu khóa một tài khoản của quản trị viên
        /// </summary>
        /// <param name="uid"> userid của tài khoản cần được khóa </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpPost("admin/block-user/{uid}")]
        [CustomAuthorization(Roles: "Admin")]
        public virtual async Task<IActionResult> AdminBlockUser(Guid uid)
        {
            ServiceResult service = await UserService.AdminBlockUser(uid);
            ApiResponse res = new(service);
            return StatusCode((int) res.StatusCode, res);
        }


        /// <summary>
        /// Xử lý yêu cầu tìm kiếm tài khoản của quản trị viên
        /// </summary>
        /// <param name="searchKey"> Từ khóa tìm kiếm </param>
        /// <param name="limit"> Thuộc tính phân trang - số lượng bản ghi cần lấy </param>
        /// <param name="offset"> Thuộc tính phân trang - Độ lệch bản ghi đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpGet("admin/search-user")]
        [CustomAuthorization(Roles: "Admin")]
        public virtual async Task<IActionResult> AdminSearchUser(string searchKey, int? limit, int? offset)
        {
            ServiceResult service = await UserService.AdminSearchUser(searchKey, limit, offset);
            ApiResponse res = new(service);
            return StatusCode((int) res.StatusCode, res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về thông tin chi tiết của một user khác của quản trị viên
        /// </summary>
        /// <param name="uid"> user id của tài khoản cần lấy thông tin </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpGet("admin/user-profile/{usernameOruid}")]
        [CustomAuthorization(Roles: "Admin")]
        public virtual async Task<IActionResult> AdminGetUserProfile(string usernameOruid)
        {
            ServiceResult service = await UserService.AdminGetUserProfile(usernameOruid);
            ApiResponse res = new(service);
            return StatusCode((int) res.StatusCode, res);
        }


        /// <summary>
        /// Xử lý yêu cầu khóa tài khoản của quản trị viên
        /// </summary>
        /// <param name="uid"> user id của tài khoản cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpDelete("admin/delete-user/{uid}")]
        [CustomAuthorization(Roles: "Admin")]
        public virtual async Task<IActionResult> AdminDeleteUser(Guid uid)
        {
            ServiceResult service = await UserService.AdminDeleteUser(uid);
            ApiResponse res = new(service);
        return StatusCode((int)res.StatusCode, res);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách user của admin
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpGet("admin/list-user")]
        [CustomAuthorization(Roles: "Admin")]
        public virtual async Task<IActionResult> AdminGetListUser(int? limit, int? offset)
        {
            ServiceResult service = await UserService.AdminGetListUser(limit, offset);
            ApiResponse res = new(service);
            return StatusCode((int)res.StatusCode, res);
        }

        #endregion

    }
}
