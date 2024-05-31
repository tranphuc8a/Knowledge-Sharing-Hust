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
    public class LessonsController(
        ILessonService lessonService   
    ) : BaseController
    {
        public ILessonService LessonService = lessonService;

        #region Anonymous

        /// <summary>
        /// Yêu cầu lấy về danh sách bài giảng
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("anonymous")]
        [AllowAnonymous]
        public async Task<IActionResult> AnonymousGetList(int? limit, int? offset, string? order, string? filter)
            => StatusCode(await LessonService.AnonymousGetPosts(
                new PaginationDto(limit, offset, ParseOrder(order), ParseFilter(filter))));

        [HttpGet("anonymous-not-convert")]
        [AllowAnonymous]
        public async Task<IActionResult> AnonymousGetListNotConverter(int? limit, int? offset, string? order, string? filter)
            => StatusCode(await LessonService.AnonymousGetPostsNotConvert(
                new PaginationDto(limit, offset, ParseOrder(order), ParseFilter(filter))));

        /// <summary>
        /// Yêu cầu lấy về danh sách bài giảng của một người dùng
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Users/anonymous/lessons/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> AnonymousGetUserList(Guid userId, int? limit, int? offset, string? order, string? filter)
            => StatusCode(await LessonService.AnonymousGetUserPosts(userId, 
                new PaginationDto(limit, offset, ParseOrder(order), ParseFilter(filter))));

        /// <summary>
        /// Yêu cầu lấy về chi tiết bài giảng
        /// </summary>
        /// <param name="lessonId"> id của lesson cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("anonymous/{lessonId}")]
        public async Task<IActionResult> AnonymousGetDetail(Guid lessonId)
            => StatusCode(await LessonService.AnonymousGetPostDetail(lessonId));

        #endregion

        #region Admin

        /// <summary>
        /// Yêu cầu admin lấy về danh sách bài giảng
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("admin")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListLessons(int? limit, int? offset, string? order, string? filter)
            => StatusCode(await LessonService.AdminGetPosts(
                new PaginationDto(limit, offset, ParseOrder(order), ParseFilter(filter))));

        /// <summary>
        /// Yêu cầu admin lấy về danh sách bài giảng của một user
        /// </summary>
        /// <param name="userId"> id của người cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Users/admin/lessons/{userId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListUserLessons(Guid userId, int? limit, int? offset, string? order, string? filter)
            => StatusCode(await LessonService.AdminGetUserPosts(userId, 
                new PaginationDto(limit, offset, ParseOrder(order), ParseFilter(filter))));

        /// <summary>
        /// Yêu cầu admin lấy về chi tiết bài giảng
        /// </summary>
        /// <param name="lessonId"> id của lesson cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("admin/{lessonId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetDetailLessons(Guid lessonId)
            => StatusCode(await LessonService.AdminGetPostDetail(lessonId));

        /// <summary>
        /// Yêu cầu admin lấy về danh sách bài giảng trong một khóa học
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Courses/admin/lessons/{courseId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListCourseLessons(Guid courseId, int? limit, int? offset, string? order, string? filter)
            => StatusCode(await LessonService.AdminGetListPostsOfCourse(courseId, 
                new PaginationDto(limit, offset, ParseOrder(order), ParseFilter(filter))));

        /// <summary>
        /// Yêu cầu admin xóa một bài giảng cụ thể
        /// </summary>
        /// <param name="lessonId"> id của bài giảng </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpDelete("admin/{lessonId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminDeleteLessons(Guid lessonId)
            => StatusCode(await LessonService.AdminDeletePost(lessonId));

        #endregion

        #region User Get

        /// <summary>
        /// Yêu cầu user get về danh sách bài giảng
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListLesson(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await LessonService.UserGetPosts(GetCurrentUserIdStrictly(), pagination));
        }

        /// <summary>
        /// Yêu cầu user get về danh sách bài giảng của user khác
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Users/lessons/{userId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListUserLesson(Guid userId, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await LessonService.UserGetUserPosts(GetCurrentUserIdStrictly(), userId, pagination));
        }

        /// <summary>
        /// Yêu cầu user get về danh sách bài giảng của chính mình
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("my")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListMyLesson(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await LessonService.UserGetMyPosts(GetCurrentUserIdStrictly(), pagination));
        }

        /// <summary>
        /// Yêu cầu user get về chi tiết một bài giảng
        /// </summary>
        /// <param name="lessonId"> id của bài giảng cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("{lessonId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetDetailLesson(Guid lessonId)
        {
            return StatusCode(await LessonService.UserGetPostDetail(GetCurrentUserIdStrictly(), lessonId));
        }

        /// <summary>
        /// Yêu cầu user get về chi tiết bài giảng của mình
        /// </summary>
        /// <param name="lessonId"> id của bài giảng cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("my/{lessonId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetDetailMyLesson(Guid lessonId)
        {
            return StatusCode(await LessonService.UserGetMyPostDetail(GetCurrentUserIdStrictly(), lessonId));
        }

        /// <summary>
        /// Yêu cầu user get về danh sách bài giảng của một khóa học
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Courses/lessons/{courseId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListCourseLesson(Guid courseId, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await LessonService.UserGetListPostsOfCourse(GetCurrentUserIdStrictly(), courseId, pagination));
        }

        /// <summary>
        /// Yêu cầu user get về danh sách bài giảng của khóa học của mình
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Courses/my/lessons/{courseId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListMyCourseLesson(Guid courseId, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await LessonService.UserGetListPostsOfMyCourse(GetCurrentUserIdStrictly(), courseId, pagination));
        }

        /// <summary>
        /// Yêu cầu user get về danh sách khóa học của một bài giảng
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <param name="lessonId"> id của bài giảng cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("my/courses/{lessonId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListMyCoursesOfALesson(Guid lessonId, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await LessonService.UserGetListCourseOfLesson(GetCurrentUserIdStrictly(), lessonId, pagination));
        }


        #endregion

        #region Get list post of a category

        /// <summary>
        /// Xử lý yêu cầu anonymous lấy về danh sách bài giảng của một category
        /// </summary>
        /// <param name="category"> Tên loại cần lấy </param>
        /// <param name="limit"> Số lượng bài giảng cần lấy </param>
        /// <param name="offset"> Độ lệch bài giảng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Categories/anonymous/lessons/{category}")]
        [AllowAnonymous]
        public async Task<IActionResult> AnonymousGetListPostsOfCategory(string category, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await LessonService.AnonymousGetListPostsOfCategory(category, pagination);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu admin lấy về danh sách bài giảng của một category
        /// </summary>
        /// <param name="category"> Tên loại cần lấy </param>
        /// <param name="limit"> Số lượng bài giảng cần lấy </param>
        /// <param name="offset"> Độ lệch bài giảng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Categories/admin/lessons/{category}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListPostsOfCategory(string category, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await LessonService.AdminGetListPostsOfCategory(category, pagination);
            return StatusCode(res);
        }


        /// <summary>
        /// Xử lý yêu cầu user lấy về danh sách bài giảng của một category
        /// </summary>
        /// <param name="category"> Tên loại cần lấy </param>
        /// <param name="limit"> Số lượng bài giảng cần lấy </param>
        /// <param name="offset"> Độ lệch bài giảng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Categories/lessons/{category}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetListPostsOfCategory(string category, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await LessonService.UserGetListPostsOfCategory(GetCurrentUserIdStrictly(), category, pagination);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu user lấy về danh sách bài giảng mà mình đã mark
        /// </summary>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Marks/my/lessons")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetListMarkedLessons(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await LessonService.UserGetMyMarkedPosts(GetCurrentUserIdStrictly(), pagination);
            return StatusCode(res);
        }

        #endregion

        #region User operations


        /// <summary>
        /// Yêu cầu user tạo mới một bài giảng
        /// </summary>
        /// <param name="model"> thông tin bài giảng mới được tạo ra </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpPost]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserAddNewLesson([FromForm] CreateLessonModel model)
        {
            return StatusCode(await LessonService.UserCreatePost(GetCurrentUserIdStrictly(), model));
        }


        /// <summary>
        /// Yêu cầu user cập nhật bài giảng
        /// </summary>
        /// <param name="lessonId"> id của bài giảng được cập nhật </param>
        /// <param name="model"> thông tin bài giảng mới được cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpPatch("{lessonId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserUpdateLesson(Guid lessonId, [FromForm] UpdateLessonModel model)
        {
            return StatusCode(await LessonService.UserUpdatePost(GetCurrentUserIdStrictly(), lessonId, model));
        }


        /// <summary>
        /// Yêu cầu user xóa bài giảng
        /// </summary>
        /// <param name="lessonId"> Id của bài giảng cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpDelete("{lessonId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserDeleteLesson(Guid lessonId)
        {
            return StatusCode(await LessonService.UserDeletePost(GetCurrentUserIdStrictly(), lessonId));
        }


        /// <summary>
        /// Yêu cầu user cập nhật quyền riêng tư của bài giảng
        /// </summary>
        /// <param name="model"> thông tin quyền riêng tư mới cần cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpPatch("privacy")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserUpdatePrivacy([FromBody] ChangeKnowledgePrivacyModel model)
        {
            return StatusCode(await LessonService.ChangePrivacy(GetCurrentUserIdStrictly(), model));
        }

        #endregion



        #region Search Apies


        /// <summary>
        /// Xử lý yêu cầu user tim kiem danh sach bai giang
        /// </summary>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <param name="filter"> Bo loc </param>
        /// <param name="order"> Bo sap xep </param>
        /// <param name="search"> Tu khoa tim kiem </param>
        /// <returns></returns>
        /// Created: PhucTV (28/3/24)
        /// Modified: None
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> UserSearchLessons(string? search, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await LessonService.UserSearchPost(GetCurrentUserId(), search, pagination);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu user tim kiem danh sach bai giang
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
        public async Task<IActionResult> UserSearchMyLessons(string? search, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await LessonService.UserSearchMyPost(GetCurrentUserIdStrictly(), search, pagination);
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
        public async Task<IActionResult> UserSearchUserLessons(Guid userId, string? search, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await LessonService.UserSearchUserPost(GetCurrentUserIdStrictly(), userId, search, pagination);
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
        public async Task<IActionResult> AdminSearchUserLessons(Guid userId, string? search, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await LessonService.AdminSearchUserPost(userId, search, pagination);
            return StatusCode(res);
        }


        #endregion
    }
}
