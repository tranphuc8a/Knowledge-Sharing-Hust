using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CreateUserItemModels;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces.Knowledges;
using KnowledgeSharingApi.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KnowledgeSharingApi.Controllers.Knowledges
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class QuestionsController(
        IQuestionService questionService
    ) : BaseController
    {
        protected readonly IQuestionService QuestionService = questionService;

        #region Anonymous Apies

        /// <summary>
        /// Yêu cầu lấy về danh sách cuộc thảo luận
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("anonymous")]
        [AllowAnonymous]
        public async Task<IActionResult> AnonymousGetList(int? limit, int? offset, string? order, string? filter)
            => StatusCode(await QuestionService.AnonymousGetPosts(
                    new PaginationDto(limit, offset, ParseOrder(order), ParseFilter(filter))));

        /// <summary>
        /// Yêu cầu lấy về danh sách cuộc thảo luận của một người dùng
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Users/anonymous/questions/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> AnonymousGetUserList(Guid userId, int? limit, int? offset, string? order, string? filter)
            => StatusCode(await QuestionService.AnonymousGetUserPosts(userId,
                    new PaginationDto(limit, offset, ParseOrder(order), ParseFilter(filter))));

        /// <summary>
        /// Yêu cầu lấy về chi tiết cuộc thảo luận
        /// </summary>
        /// <param name="questionId"> id của question cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("anonymous/{questionId}")]
        public async Task<IActionResult> AnonymousGetDetail(Guid questionId)
            => StatusCode(await QuestionService.AnonymousGetPostDetail(questionId));

        #endregion


        #region Admin Apies

        /// <summary>
        /// Yêu cầu admin lấy về danh sách cuộc thảo luận
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("admin")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListQuestion(int? limit, int? offset, string? order, string? filter)
            => StatusCode(await QuestionService.AdminGetPosts(
                    new PaginationDto(limit, offset, ParseOrder(order), ParseFilter(filter))));

        /// <summary>
        /// Yêu cầu admin lấy về danh sách cuộc thảo luận của một user
        /// </summary>
        /// <param name="userId"> id của người cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Users/admin/questions/{userId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListUserQuestion(Guid userId, int? limit, int? offset, string? order, string? filter)
            => StatusCode(await QuestionService.AdminGetUserPosts(userId,
                    new PaginationDto(limit, offset, ParseOrder(order), ParseFilter(filter))));

        /// <summary>
        /// Yêu cầu admin lấy về chi tiết cuộc thảo luận
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("admin/{questionId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetDetailQuestion(Guid questionId)
            => StatusCode(await QuestionService.AdminGetPostDetail(questionId));

        /// <summary>
        /// Yêu cầu admin lấy về danh sách cuộc thảo luận trong một khóa học
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Courses/admin/questions/{courseId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListCourseQuestion(Guid courseId, int? limit, int? offset, string? order, string? filter)
            => StatusCode(await QuestionService.AdminGetListPostsOfCourse(courseId,
                    new PaginationDto(limit, offset, ParseOrder(order), ParseFilter(filter))));

        /// <summary>
        /// Yêu cầu admin xóa một cuộc thảo luận cụ thể
        /// </summary>
        /// <param name="questionId"> id của cuộc thảo luận </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpDelete("admin/{questionId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminDeleteQuestion(Guid questionId)
            => StatusCode(await QuestionService.AdminDeletePost(questionId));


        #endregion


        #region User Apies

        #region User get Apies

        /// <summary>
        /// Yêu cầu user get về danh sách cuộc thảo luận
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListQuestion(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await QuestionService.UserGetPosts(GetCurrentUserIdStrictly(), pagination));
        }

        /// <summary>
        /// Yêu cầu user get về danh sách cuộc thảo luận của user khác
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Users/questions/{userId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListUserQuestion(Guid userId, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await QuestionService.UserGetUserPosts(GetCurrentUserIdStrictly(), userId, pagination));
        }

        /// <summary>
        /// Yêu cầu user get về danh sách cuộc thảo luận của chính mình
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("my")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListMyQuestion(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await QuestionService.UserGetMyPosts(GetCurrentUserIdStrictly(), pagination));
        }

        /// <summary>
        /// Yêu cầu user get về chi tiết một cuộc thảo luận
        /// </summary>
        /// <param name="questionId"> id của cuộc thảo luận cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("{questionId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetDetailQuestion(Guid questionId)
        {
            return StatusCode(await QuestionService.UserGetPostDetail(GetCurrentUserIdStrictly(), questionId));
        }

        /// <summary>
        /// Yêu cầu user get về chi tiết cuộc thảo luận của mình
        /// </summary>
        /// <param name="questionId"> id của cuộc thảo luận cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("my/{questionId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetDetailMyQuestion(Guid questionId)
        {
            return StatusCode(await QuestionService.UserGetMyPostDetail(GetCurrentUserIdStrictly(), questionId));
        }

        /// <summary>
        /// Yêu cầu user get về danh sách cuộc thảo luận của một khóa học
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Courses/questions/{courseId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListCourseQuestion(Guid courseId, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await QuestionService.UserGetListPostsOfCourse(GetCurrentUserIdStrictly(), courseId, pagination));
        }

        /// <summary>
        /// Yêu cầu user get về danh sách cuộc thảo luận của khóa học của mình
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Courses/my/questions/{courseId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListMyCourseQuestion(Guid courseId, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await QuestionService.UserGetListPostsOfMyCourse(GetCurrentUserIdStrictly(), courseId, pagination));
        }


        #endregion


        /// <summary>
        /// Yêu cầu user tạo mới một cuộc thảo luận
        /// </summary>
        /// <param name="model"> thông tin cuộc thảo luận mới được tạo ra </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpPost]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserAddNewQuestion([FromForm] CreateQuestionModel model)
        {
            return StatusCode(await QuestionService.UserCreatePost(GetCurrentUserIdStrictly(), model));
        }


        /// <summary>
        /// Yêu cầu user cập nhật cuộc thảo luận
        /// </summary>
        /// <param name="questionId"> id của cuộc thảo luận được cập nhật </param>
        /// <param name="model"> thông tin cuộc thảo luận mới được cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpPatch("{questionId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserUpdateQuestion(Guid questionId, [FromForm] UpdateQuestionModel model)
        {
            return StatusCode(await QuestionService.UserUpdatePost(GetCurrentUserIdStrictly(), questionId, model));
        }


        /// <summary>
        /// Yêu cầu user xóa cuộc thảo luận
        /// </summary>
        /// <param name="questionId"> Id của cuộc thảo luận cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpDelete("{questionId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserDeleteQuestion(Guid questionId)
        {
            return StatusCode(await QuestionService.UserDeletePost(GetCurrentUserIdStrictly(), questionId));
        }


        /// <summary>
        /// Yêu cầu user xác minh cuộc thảo luận
        /// </summary>
        /// <param name="questionId"> id của cuộc thảo luận </param>
        /// <param name="isConfirm"> true - Chấp nhận, false - Không chấp nhận </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpPost("confirm/{questionId}/{isConfirm}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserConfirmQuestion(Guid questionId, bool isConfirm)
        {
            return StatusCode(await QuestionService.ConfirmQuestion(GetCurrentUserIdStrictly(), questionId, isConfirm));
        }

        #endregion


        #region Get list post of a category

        /// <summary>
        /// Xử lý yêu cầu anonymous lấy về danh sách bài thảo luận của một category
        /// </summary>
        /// <param name="category"> Tên loại cần lấy </param>
        /// <param name="limit"> Số lượng bài thảo luận cần lấy </param>
        /// <param name="offset"> Độ lệch bài thảo luận đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Categories/anonymous/questions/{category}")]
        [AllowAnonymous]
        public async Task<IActionResult> AnonymousGetListPostsOfCategory(string category, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await QuestionService.AnonymousGetListPostsOfCategory(category, pagination);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu admin lấy về danh sách bài thảo luận của một category
        /// </summary>
        /// <param name="category"> Tên loại cần lấy </param>
        /// <param name="limit"> Số lượng bài thảo luận cần lấy </param>
        /// <param name="offset"> Độ lệch bài thảo luận đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Categories/admin/questions/{category}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListPostsOfCategory(string category, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await QuestionService.AdminGetListPostsOfCategory(category, pagination);
            return StatusCode(res);
        }


        /// <summary>
        /// Xử lý yêu cầu user lấy về danh sách bài thảo luận của một category
        /// </summary>
        /// <param name="category"> Tên loại cần lấy </param>
        /// <param name="limit"> Số lượng bài thảo luận cần lấy </param>
        /// <param name="offset"> Độ lệch bài thảo luận đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Categories/questions/{category}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetListPostsOfCategory(string category, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await QuestionService.UserGetListPostsOfCategory(GetCurrentUserIdStrictly(), category, pagination);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu user lấy về danh sách bài thảo luận mà mình đã mark
        /// </summary>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Marks/my/questions")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetListMarkedQuestions(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await QuestionService.UserGetMyMarkedPosts(GetCurrentUserIdStrictly(), pagination);
            return StatusCode(res);
        }

        #endregion


        #region Search Question apis
        /// <summary>
        /// Xử lý yêu cầu user tim kiem danh sach bai thao luan
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
        public async Task<IActionResult> UserSearchQuestions(string? search, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await QuestionService.UserSearchPost(GetCurrentUserId(), search, pagination);
            return StatusCode(res);
        }


        /// <summary>
        /// Xử lý yêu cầu user tim kiem danh sach bai thao luan trong mot khoa hoc
        /// </summary>
        /// <param name="courseId"> id khoa hoc can tim kiem bai thao luan </param>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <param name="filter"> Bo loc </param>
        /// <param name="order"> Bo sap xep </param>
        /// <param name="search"> Tu khoa tim kiem </param>
        /// <returns></returns>
        /// Created: PhucTV (23/5/24)
        /// Modified: None
        [HttpGet("/api/v1/Courses/search/questions/{courseId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserSearchQuestions(Guid courseId, string? search, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await QuestionService.UserSearchQuestionOfCourse(GetCurrentUserIdStrictly(), courseId, search, pagination);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu user tim kiem danh sach bai thao luan
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
        public async Task<IActionResult> UserSearchMyQuestions(string? search, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await QuestionService.UserSearchMyPost(GetCurrentUserIdStrictly(), search, pagination);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu user lấy về danh sách bài thảo luận mà mình đã mark
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
        public async Task<IActionResult> UserSearchUserQuestions(Guid userId, string? search, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await QuestionService.UserSearchUserPost(GetCurrentUserIdStrictly(), userId, search, pagination);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu user lấy về danh sách bài thảo luận mà mình đã mark
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
        public async Task<IActionResult> AdminSearchUserQuestions(Guid userId, string? search, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await QuestionService.AdminSearchUserPost(userId, search, pagination);
            return StatusCode(res);
        }
        #endregion

    }
}
