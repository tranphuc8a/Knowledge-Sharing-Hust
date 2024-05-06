using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CreateUserItemModels;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels;
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
    public class QuestionsController(
        IQuestionService questionService
    ) : ControllerBase
    {
        protected readonly IQuestionService QuestionService = questionService;

        protected virtual IActionResult StatusCode(ServiceResult result)
        {
            return StatusCode((int)result.StatusCode, new ApiResponse(result));
        }

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
        public async Task<IActionResult> AnonymousGetList(int? limit, int? offset)
            => StatusCode(await QuestionService.AnonymousGetPosts(limit, offset));

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
        public async Task<IActionResult> AnonymousGetUserList(Guid userId, int? limit, int? offset)
            => StatusCode(await QuestionService.AnonymousGetUserPosts(userId, limit, offset));

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
        public async Task<IActionResult> AdminGetListQuestion(int? limit, int? offset)
            => StatusCode(await QuestionService.AdminGetPosts(limit, offset));

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
        public async Task<IActionResult> AdminGetListUserQuestion(Guid userId, int? limit, int? offset)
            => StatusCode(await QuestionService.AdminGetUserPosts(userId, limit, offset));

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
        public async Task<IActionResult> AdminGetListCourseQuestion(Guid courseId, int? limit, int? offset)
            => StatusCode(await QuestionService.AdminGetListPostsOfCourse(courseId, limit, offset));

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
        public async Task<IActionResult> UserGetListQuestion(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await QuestionService.UserGetPosts(Guid.Parse(myUid), limit, offset));
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
        [HttpGet("/api/v1/user/questions/{userId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListUserQuestion(Guid userId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await QuestionService.UserGetUserPosts(Guid.Parse(myUid), userId, limit, offset));
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
        public async Task<IActionResult> UserGetListMyQuestion(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await QuestionService.UserGetMyPosts(Guid.Parse(myUid), limit, offset));
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
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await QuestionService.UserGetPostDetail(Guid.Parse(myUid), questionId));
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
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await QuestionService.UserGetMyPostDetail(Guid.Parse(myUid), questionId));
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
        public async Task<IActionResult> UserGetListCourseQuestion(Guid courseId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await QuestionService.UserGetListPostsOfCourse(Guid.Parse(myUid), courseId, limit, offset));
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
        public async Task<IActionResult> UserGetListMyCourseQuestion(Guid courseId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await QuestionService.UserGetListPostsOfMyCourse(Guid.Parse(myUid), courseId, limit, offset));
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
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await QuestionService.UserCreatePost(Guid.Parse(myUid), model));
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
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await QuestionService.UserUpdatePost(Guid.Parse(myUid), questionId, model));
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
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await QuestionService.UserDeletePost(Guid.Parse(myUid), questionId));
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
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await QuestionService.ConfirmQuestion(Guid.Parse(myUid), questionId, isConfirm));
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
        public async Task<IActionResult> AnonymousGetListPostsOfCategory(string category, int? limit, int? offset)
        {
            ServiceResult res = await QuestionService.AnonymousGetListPostsOfCategory(category, limit, offset);
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
        public async Task<IActionResult> AdminGetListPostsOfCategory(string category, int? limit, int? offset)
        {
            ServiceResult res = await QuestionService.AdminGetListPostsOfCategory(category, limit, offset);
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
        public async Task<IActionResult> UserGetListPostsOfCategory(string category, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await QuestionService.UserGetListPostsOfCategory(Guid.Parse(myUid), category, limit, offset);
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
        public async Task<IActionResult> UserGetListMarkedQuestions(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await QuestionService.UserGetMyMarkedPosts(Guid.Parse(myUid), limit, offset);
            return StatusCode(res);
        }

        #endregion

    }
}
