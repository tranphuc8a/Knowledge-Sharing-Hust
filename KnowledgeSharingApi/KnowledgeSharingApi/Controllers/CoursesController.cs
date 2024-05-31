using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CourseLessonModels;
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
    public class CoursesController(ICourseService courseService) : BaseController
    {
        protected readonly ICourseService CourseService = courseService;

        #region Anonymous APies

        /// <summary>
        /// Yêu cầu lấy về danh sách khóa học
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("anonymous")]
        [AllowAnonymous]
        public async Task<IActionResult> AnonymousGetList(int? limit, int? offset, string? order, string? filter)
            => StatusCode(await CourseService.AnonymousGetCourses(
                new PaginationDto(limit, offset, ParseOrder(order), ParseFilter(filter))));

        /// <summary>
        /// Yêu cầu lấy về danh sách khóa học của một người dùng
        /// </summary>
        /// <param name="userId"> id của user cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Users/anonymous/courses/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> AnonymousGetUserList(Guid userId, int? limit, int? offset, string? order, string? filter)
            => StatusCode(await CourseService.AnonymousGetUserCourses(userId, 
                new PaginationDto(limit, offset, ParseOrder(order), ParseFilter(filter))));

        /// <summary>
        /// Yêu cầu lấy về chi tiết khóa học
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("anonymous/{courseId}")]
        public async Task<IActionResult> AnonymousGetDetail(Guid courseId)
            => StatusCode(await CourseService.AnonymousGetCourseDetail(courseId));

        #endregion

        #region Admin apies

        /// <summary>
        /// Yêu cầu admin lấy về danh sách khóa học
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("admin")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListCourses(int? limit, int? offset, string? order, string? filter)
            => StatusCode(await CourseService.AdminGetCourses(
                new PaginationDto(limit, offset, ParseOrder(order), ParseFilter(filter))));

        /// <summary>
        /// Yêu cầu admin lấy về danh sách khóa học của một user
        /// </summary>
        /// <param name="userId"> id của người cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Users/admin/courses/{userId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListUserCourses(Guid userId, int? limit, int? offset, string? order, string? filter)
            => StatusCode(await CourseService.AdminGetUserCourses(userId, 
                new PaginationDto(limit, offset, ParseOrder(order), ParseFilter(filter))));

        /// <summary>
        /// Yêu cầu admin lấy về chi tiết khóa học
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("admin/{courseId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetDetailCourses(Guid courseId)
            => StatusCode(await CourseService.AdminGetCourseDetail(courseId));

        /// <summary>
        /// Yêu cầu admin lấy về danh sách khóa học đã mà một user đã đăng ký
        /// </summary>
        /// <param name="userId"> Id của user cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Users/admin/registered-courses/{userId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetUserRegisteredCourses(Guid userId, int? limit, int? offset, string? order, string? filter)
            => StatusCode(await CourseService.AdminGetUserRegisteredCourses(userId, 
                new PaginationDto(limit, offset, ParseOrder(order), ParseFilter(filter))));

        /// <summary>
        /// Yêu cầu admin xóa một khóa học cụ thể
        /// </summary>
        /// <param name="courseId"> id của khóa học </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpDelete("admin/{courseId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminDeleteCourses(Guid courseId)
            => StatusCode(await CourseService.AdminDeleteCourse(courseId));

        #endregion


        #region User categories and marked apies

        /// <summary>
        /// Xử lý yêu cầu anonymous lấy về danh sách khóa học của một category
        /// </summary>
        /// <param name="category"> Tên loại cần lấy </param>
        /// <param name="limit"> Số lượng khóa học cần lấy </param>
        /// <param name="offset"> Độ lệch khóa học đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Categories/anonymous/courses/{category}")]
        [AllowAnonymous]
        public async Task<IActionResult> AnonymousGetListCoursesOfCategory(string category, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await CourseService.AnonymousGetListCourseOfCategory(category, pagination);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu admin lấy về danh sách khóa học của một category
        /// </summary>
        /// <param name="category"> Tên loại cần lấy </param>
        /// <param name="limit"> Số lượng khóa học cần lấy </param>
        /// <param name="offset"> Độ lệch khóa học đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Categories/admin/courses/{category}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListCoursesOfCategory(string category, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await CourseService.AdminListCourseOfCategory(category, pagination);
            return StatusCode(res);
        }


        /// <summary>
        /// Xử lý yêu cầu user lấy về danh sách khóa học của một category
        /// </summary>
        /// <param name="category"> Tên loại cần lấy </param>
        /// <param name="limit"> Số lượng khóa học cần lấy </param>
        /// <param name="offset"> Độ lệch khóa học đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Categories/courses/{category}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetListCoursesOfCategory(string category, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await CourseService.UserGetListCourseOfCategory(GetCurrentUserIdStrictly(), category, pagination);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu user lấy về danh sách khóa học mà mình đã mark
        /// </summary>
        /// <param name="limit"> Số lượng khóa học cần lấy </param>
        /// <param name="offset"> Độ lệch khóa học đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Marks/my/courses")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetListMarkedCourses(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await CourseService.UserGetMarkedCourses(GetCurrentUserIdStrictly(), pagination);
            return StatusCode(res);
        }

        #endregion


        #region User Get

        /// <summary>
        /// Yêu cầu user get về danh sách khóa học
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListCourse(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await CourseService.UserGetListCourses(GetCurrentUserIdStrictly(), pagination));
        }

        /// <summary>
        /// Yêu cầu user get về danh sách khóa học của user khác
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Users/courses/{userId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListUserCourse(Guid userId, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await CourseService.UserGetUserCourses(GetCurrentUserIdStrictly(), userId, pagination));
        }

        /// <summary>
        /// Yêu cầu user get về danh sách khóa học của chính mình
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("my")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListMyCourse(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await CourseService.UserGetMyCourses(GetCurrentUserIdStrictly(), pagination));
        }

        /// <summary>
        /// Yêu cầu user get về chi tiết một khóa học
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("{courseId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetDetailCourse(Guid courseId)
        {
            return StatusCode(await CourseService.UserGetCourseDetail(GetCurrentUserIdStrictly(), courseId));
        }

        /// <summary>
        /// Yêu cầu user get về chi tiết khóa học của mình
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("my/{courseId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetDetailMyCourse(Guid courseId)
        {
            return StatusCode(await CourseService.UserGetMyCourseDetail(GetCurrentUserIdStrictly(), courseId));
        }


        /// <summary>
        /// Yêu cầu user get về danh sách khóa học mà mình đã đăng ký
        /// </summary>
        /// <param name="limit"> Kích thước trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("my/registered-courses")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetMyRegisteredCourse(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await CourseService.UserGetMyRegisteredCourses(GetCurrentUserIdStrictly(), pagination));
        }

        #endregion


        #region User operations


        /// <summary>
        /// Yêu cầu user tạo mới một khóa học
        /// </summary>
        /// <param name="model"> thông tin khóa học mới được tạo ra </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpPost]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserAddNewCourse([FromForm] CreateCourseModel model)
        {
            return StatusCode(await CourseService.UserCreateCourse(GetCurrentUserIdStrictly(), model));
        }


        /// <summary>
        /// Yêu cầu user cập nhật thông tin của một khóa học
        /// </summary>
        /// <param name="courseId"> id của khóa học được cập nhật </param>
        /// <param name="model"> thông tin khóa học mới được cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpPatch("{courseId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserUpdateCourse(Guid courseId, [FromForm] UpdateCourseModel model)
        {
            return StatusCode(await CourseService.UserUpdateCourse(GetCurrentUserIdStrictly(), courseId, model));
        }


        /// <summary>
        /// Yêu cầu user xóa khóa học
        /// </summary>
        /// <param name="courseId"> Id của khóa học cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpDelete("{courseId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserDeleteCourse(Guid courseId)
        {
            return StatusCode(await CourseService.UserDeleteCourse(GetCurrentUserIdStrictly(), courseId));
        }


        /// <summary>
        /// Yêu cầu user cập nhật quyền riêng tư của khóa học
        /// </summary>
        /// <param name="model"> thông tin quyền riêng tư mới cần cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpPatch("privacy")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserUpdatePrivacy([FromBody] ChangeKnowledgePrivacyModel model)
        {
            return StatusCode(await CourseService.ChangePrivacy(GetCurrentUserIdStrictly(), model));
        }

        #endregion




        #region Search Course apis
        /// <summary>
        /// Xử lý yêu cầu user tim kiem danh sach khoa hoc
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
        public async Task<IActionResult> UserSearchCourses(string? search, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await CourseService.UserSearchCourse(GetCurrentUserId(), search, pagination);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu user tim kiem danh sach khoa hoc
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
        public async Task<IActionResult> UserSearchMyCourses(string? search, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await CourseService.UserSearchMyCourse(GetCurrentUserIdStrictly(), search, pagination);
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
        public async Task<IActionResult> UserSearchUserCourses(Guid userId, string? search, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await CourseService.UserSearchUserCourse(GetCurrentUserIdStrictly(), userId, search, pagination);
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
        public async Task<IActionResult> AdminSearchUserCourses(Guid userId, string? search, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            ServiceResult res = await CourseService.AdminSearchUserCourse(userId, search, pagination);
            return StatusCode(res);
        }
        #endregion
    }
}
