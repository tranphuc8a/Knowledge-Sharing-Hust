using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KnowledgeSharingApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PostsController(
        IPostService postService    
    ) : ControllerBase
    {
        protected readonly IPostService PostService = postService;

        protected virtual IActionResult StatusCode(ServiceResult result)
        {
            return StatusCode((int)result.StatusCode, new ApiResponse(result));
        }

        #region Anonymous APIes
        
        /// <summary>
        /// Xử lý yêu cầu lấy về danh sách bài đăng
        /// </summary>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        [HttpGet("anonymous")]
        public async Task<IActionResult> AnonymousGetListPosts(int? limit, int? offset)
        {
            ServiceResult res = await PostService.AnonymousGetPosts(limit, offset);
            return StatusCode(res);
        }


        /// <summary>
        /// Xử lý yêu cầu anonymous lấy về danh sách bài đăng của một user
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Users/anonymous/posts/{userId}")]
        public async Task<IActionResult> AnonymousGetListUserPosts(Guid userId, int? limit, int? offset)
        {
            ServiceResult res = await PostService.AnonymousGetUserPosts(userId, limit, offset);
            return StatusCode(res);
        }

        #endregion


        #region Admin APIes

        /// <summary>
        /// Xử lý yêu cầu admin lấy về danh sách bài đăng
        /// </summary>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        [HttpGet("admin")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListPosts(int? limit, int? offset)
        {
            ServiceResult res = await PostService.AdminGetPosts(limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu Admin lấy về danh sách bài đăng của một user
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Users/admin/posts/{userId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListUserPosts(Guid userId, int? limit, int? offset)
        {
            ServiceResult res = await PostService.AdminGetUserPosts(userId, limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu Admin Xóa một bài đăng
        /// </summary>
        /// <param name="postId"> id bài đăng cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        [HttpDelete("admin/{postId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminDeletePost(Guid postId)
        {
            ServiceResult res = await PostService.AdminDeletePost(postId);
            return StatusCode(res);
        }

        #endregion


        #region User APIes

        /// <summary>
        /// Xử lý yêu cầu user lấy về danh sách bài đăng
        /// </summary>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        [HttpGet]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListPosts(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await PostService.UserGetPosts(Guid.Parse(myUid), limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu user lấy về danh sách bài đăng của mình
        /// </summary>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        [HttpGet("my")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListMyPosts(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await PostService.UserGetMyPosts(Guid.Parse(myUid), limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu user lấy về danh sách bài đăng của user khác
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Users/posts/{userId}")]
        [CustomAuthorization(Roles: "Adminm, User")]
        public async Task<IActionResult> UserGetPost(Guid userId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await PostService.UserGetUserPosts(Guid.Parse(myUid), userId, limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu user xóa bài đăng của mình
        /// </summary>
        /// <param name="postId"> id của bài đăng cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        [HttpDelete("{postId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> AdminGetListPosts(Guid postId)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await PostService.UserDeletePost(Guid.Parse(myUid), postId);
            return StatusCode(res);
        }

        #endregion

        #region Get list post of a category

        /// <summary>
        /// Xử lý yêu cầu anonymous lấy về danh sách bài đăng của một category
        /// </summary>
        /// <param name="category"> Tên loại cần lấy </param>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Categories/anonymous/posts/{category}")]
        [AllowAnonymous]
        public async Task<IActionResult> AnonymousGetListPostsOfCategory(string category, int? limit, int? offset)
        {
            ServiceResult res = await PostService.AnonymousGetListPostsOfCategory(category, limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu admin lấy về danh sách bài đăng của một category
        /// </summary>
        /// <param name="category"> Tên loại cần lấy </param>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Categories/admin/posts/{category}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListPostsOfCategory(string category, int? limit, int? offset)
        {
            ServiceResult res = await PostService.AdminGetListPostsOfCategory(category, limit, offset);
            return StatusCode(res);
        }


        /// <summary>
        /// Xử lý yêu cầu user lấy về danh sách bài đăng của một category
        /// </summary>
        /// <param name="category"> Tên loại cần lấy </param>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Categories/posts/{category}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetListPostsOfCategory(string category, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await PostService.UserGetListPostsOfCategory(Guid.Parse(myUid), category, limit, offset);
            return StatusCode(res);
        }


        /// <summary>
        /// Xử lý yêu cầu user lấy về danh sách bài đăng mà mình đã mark
        /// </summary>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Marks/my/posts")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetListMarkedPosts(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await PostService.UserGetMyMarkedPosts(Guid.Parse(myUid), limit, offset);
            return StatusCode(res);
        }
        #endregion
    }
}
