using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UserProfileModels;
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
    public class UsersController : BaseController
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
        [CustomAuthorization("User, Admin")]
        public virtual async Task<IActionResult> GetMeProfile()
        {
            string? uId = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier);
            if (uId == null) return FailedAuthentication(HttpContext.User);
            ServiceResult service = await UserService.GetMyUserProfile(Guid.Parse(uId));
            return StatusCode(service);
        }

        /// <summary>
        /// Xử lý yêu cầu cập nhật ảnh đại diện
        /// </summary
        /// <param name="avatar"> IFormFile - file ảnh đại điện được upload lên </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpPatch("me/update-avatar-file")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> UpdateMeAvatarByFile([FromForm] UploadImageModel avatar)
        {
            ServiceResult service = await UserService.UpdateMyAvatarImage(GetCurrentUserIdStrictly(), avatar);
            return StatusCode(service);
        }

        /// <summary>
        /// Xử lý yêu cầu cập nhật ảnh đại diện
        /// </summary
        /// <param name="avatar"> string - url ảnh đại điện được upload lên </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpPatch("me/update-avatar-url")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> UpdateMeProfile([FromBody] string? avatar)
        {
            ServiceResult service = await UserService.UpdateMyAvatarUrl(GetCurrentUserIdStrictly(), avatar);
            return StatusCode(service);
        }

        /// <summary>
        /// Xử lý yêu cầu cập nhật ảnh bìa
        /// </summary>
        /// <param name="cover"> IFormFile - nội dung file ảnh bìa </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpPatch("me/update-cover-file")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> UpdateMeAvatar([FromForm] UploadImageModel cover)
        {
            ServiceResult service = await UserService.UpdateMyCoverImage(GetCurrentUserIdStrictly(), cover);
            return StatusCode(service);
        }

        /// <summary>
        /// Xử lý yêu cầu cập nhật ảnh bìa
        /// </summary>
        /// <param name="cover"> string - url nội dung file ảnh bìa </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpPatch("me/update-cover-url")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> UpdateMeCover([FromBody] string? cover)
        {
            ServiceResult service = await UserService.UpdateMyCoverUrl(GetCurrentUserIdStrictly(), cover);
            return StatusCode(service);
        }

        /// <summary>
        /// Xử lý yêu cầu cập nhật tên hiển thị
        /// </summary>
        /// <param name="fullname"> string - tên mới </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpPatch("me/update-fullname")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> UpdateMeFullname([FromBody] string? fullname)
        {
            ServiceResult service = await UserService.UpdateMyFullname(GetCurrentUserIdStrictly(), fullname);
            return StatusCode(service);
        }

        /// <summary>
        /// Xử lý yêu cầu cập nhật thông tin chung
        /// </summary>
        /// <param name="generalInfor"> thông tin chung (url avatar, url cover, fullname) cần cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpPatch("me/update-general-info")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> UpdateMeGeneralInfor([FromBody] UpdateGeneralInforModel model)
        {
            ServiceResult service = await UserService.UpdateMyGeneralInfo(GetCurrentUserIdStrictly(), model);
            return StatusCode(service);
        }

        /// <summary>
        /// Xử lý yêu cầu cập nhật bio
        /// </summary>
        /// <param name="bio"> Bio cần update </param>
        /// <returns></returns>
        /// Created: PhucTV (8/5/24)
        /// Modified: None
        [HttpPatch("me/update-bio")]
        [CustomAuthorization(Roles: "User, Admin")]
        public virtual async Task<IActionResult> UpdateMeBio([FromBody] string? bio)
        {
            ServiceResult service = await UserService.UpdateMyBio(GetCurrentUserIdStrictly(), bio);
            return StatusCode(service);
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
            ServiceResult service = await UserService.UpdateMyUserProfile(GetCurrentUserIdStrictly(), model);
            return StatusCode(service);
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
            ServiceResult service = await UserService.GetUserDetail(GetCurrentUserIdStrictly(), unOruid);
            return StatusCode(service);
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
        public virtual async Task<IActionResult> SearchMe(string searchKey, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult service = await UserService.SearchUser(GetCurrentUserIdStrictly(), searchKey, pagination);
            return StatusCode(service);
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
            return StatusCode((int) res.StatusCode, res);
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
            return StatusCode(service);
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
            return StatusCode(service);
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
            return StatusCode(service);
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
        public virtual async Task<IActionResult> AdminSearchUser(string searchKey, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult service = await UserService.AdminSearchUser(searchKey, pagination);
            return StatusCode(service);
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
            return StatusCode(service);
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
        return StatusCode(service);
        }

        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách user của admin
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (17/3/24)
        /// Modified: None
        [HttpGet("admin/list-user")]
        [CustomAuthorization(Roles: "Admin")]
        public virtual async Task<IActionResult> AdminGetListUser(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult service = await UserService.AdminGetListUser(pagination);
            return StatusCode(service);
        }

        #endregion

    }
}
