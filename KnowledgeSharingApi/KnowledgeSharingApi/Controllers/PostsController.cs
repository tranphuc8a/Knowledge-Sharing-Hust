using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces;
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
        [HttpGet("/user/anonymous/posts/{userId}")]
        public async Task<IActionResult> AnonymousGetListUserPosts(string userId, int? limit, int? offset)
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
        [HttpGet("/user/admin/posts/{userId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListUserPosts(string userId, int? limit, int? offset)
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
        public async Task<IActionResult> AdminDeletePost(string postId)
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
            ServiceResult res = await PostService.UserGetPosts(myUid, limit, offset);
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
            ServiceResult res = await PostService.UserGetMyPosts(myUid, limit, offset);
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
        [HttpGet("/user/posts/{userId}")]
        [CustomAuthorization(Roles: "Adminm, User")]
        public async Task<IActionResult> UserGetPost(string userId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await PostService.UserGetUserPosts(myUid, userId, limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu user xóa bài đăng của mình
        /// </summary>
        /// <param name="postId"> id của bài đăng cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (23/3/24)
        /// Modified: None
        [HttpGet("{postId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> AdminGetListPosts(string postId)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await PostService.UserDeletePost(myUid, postId);
            return StatusCode(res);
        }

        #endregion
    }
}
