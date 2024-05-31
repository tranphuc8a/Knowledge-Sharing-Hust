using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces;
using KnowledgeSharingApi.Services.Services;
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
    ) : BaseController
    {
        protected readonly IPostService PostService = postService;

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
        public async Task<IActionResult> AnonymousGetListPosts(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await PostService.AnonymousGetPosts(pagination);
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
        public async Task<IActionResult> AnonymousGetListUserPosts(Guid userId, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await PostService.AnonymousGetUserPosts(userId, pagination);
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
        public async Task<IActionResult> AdminGetListPosts(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await PostService.AdminGetPosts(pagination);
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
        public async Task<IActionResult> AdminGetListUserPosts(Guid userId, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await PostService.AdminGetUserPosts(userId, pagination);
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
        public async Task<IActionResult> UserGetListPosts(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await PostService.UserGetPosts(GetCurrentUserIdStrictly(), pagination);
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
        public async Task<IActionResult> UserGetListMyPosts(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await PostService.UserGetMyPosts(GetCurrentUserIdStrictly(), pagination);
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
        public async Task<IActionResult> UserGetPost(Guid userId, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await PostService.UserGetUserPosts(GetCurrentUserIdStrictly(), userId, pagination);
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
            ServiceResult res = await PostService.UserDeletePost(GetCurrentUserIdStrictly(), postId);
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
        public async Task<IActionResult> AnonymousGetListPostsOfCategory(string category, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await PostService.AnonymousGetListPostsOfCategory(category, pagination);
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
        public async Task<IActionResult> AdminGetListPostsOfCategory(string category, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await PostService.AdminGetListPostsOfCategory(category, pagination);
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
        public async Task<IActionResult> UserGetListPostsOfCategory(string category, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await PostService.UserGetListPostsOfCategory(GetCurrentUserIdStrictly(), category, pagination);
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
        public async Task<IActionResult> UserGetListMarkedPosts(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await PostService.UserGetMyMarkedPosts(GetCurrentUserIdStrictly(), pagination);
            return StatusCode(res);
        }
        #endregion



        #region Search Apies


        /// <summary>
        /// Xử lý yêu cầu user tim kiem danh sach bai post
        /// </summary>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <param name="filter"> Bo loc </param>
        /// <param name="order"> Bo sap xep </param>
        /// <param name="search"> Tu khoa tim kiem </param>
        /// <returns></returns>
        /// Created: PhucTV (19/5/24)
        /// Modified: None
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> UserSearchPosts(string? search, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await PostService.UserSearchPost(GetCurrentUserId(), search, pagination);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu user tim kiem danh sach bai post
        /// </summary>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <param name="filter"> Bo loc </param>
        /// <param name="order"> Bo sap xep </param>
        /// <param name="search"> Tu khoa tim kiem </param>
        /// <returns></returns>
        /// Created: PhucTV (19/5/24)
        /// Modified: None
        [HttpGet("search/my")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserSearchMyPosts(string? search, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await PostService.UserSearchMyPost(GetCurrentUserIdStrictly(), search, pagination);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu user lấy về danh sách bài giảng mà mình đã mark
        /// </summary>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <param name="filter"> Bo loc </param>
        /// <param name="order"> Bo sap xep </param>
        /// <param name="search"> Tu khoa tim kiem </param>
        /// <param name="userId"> id user can lay </param>
        /// <returns></returns>
        /// Created: PhucTV (19/5/24)
        /// Modified: None
        [HttpGet("search/user/{userId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserSearchUserPosts(Guid userId, string? search, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await PostService.UserSearchUserPost(GetCurrentUserIdStrictly(), userId, search, pagination);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu user lấy về danh sách bài giảng mà mình đã mark
        /// </summary>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <param name="filter"> Bo loc </param>
        /// <param name="order"> Bo sap xep </param>
        /// <param name="search"> Tu khoa tim kiem </param>
        /// <param name="userId"> id user can lay </param>
        /// <returns></returns>
        /// Created: PhucTV (19/5/24)
        /// Modified: None
        [HttpGet("search/admin/user/{userId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminSearchUserPosts(Guid userId, string? search, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await PostService.AdminSearchUserPost(userId, search, pagination);
            return StatusCode(res);
        }


        #endregion
    }
}
