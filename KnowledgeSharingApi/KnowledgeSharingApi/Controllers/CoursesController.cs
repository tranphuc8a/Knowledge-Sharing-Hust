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
    public class CoursesController(ICourseService courseService, ICourseLessonService courseLessonService) : ControllerBase
    {
        protected readonly ICourseService CourseService = courseService;
        protected readonly ICourseLessonService CourseLessonService = courseLessonService;

        protected virtual IActionResult StatusCode(ServiceResult result)
        {
            return StatusCode((int)result.StatusCode, new ApiResponse(result));
        }

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
        public async Task<IActionResult> AnonymousGetList(int? limit, int? offset)
            => StatusCode(await CourseService.AnonymousGetCourses(limit, offset));

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
        public async Task<IActionResult> AnonymousGetUserList(Guid userId, int? limit, int? offset)
            => StatusCode(await CourseService.AnonymousGetUserCourses(userId, limit, offset));

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
        public async Task<IActionResult> AdminGetListCourses(int? limit, int? offset)
            => StatusCode(await CourseService.AdminGetCourses(limit, offset));

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
        public async Task<IActionResult> AdminGetListUserCourses(Guid userId, int? limit, int? offset)
            => StatusCode(await CourseService.AdminGetUserCourses(userId, limit, offset));

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
        public async Task<IActionResult> AdminGetUserRegisteredCourses(Guid userId, int? limit, int? offset)
            => StatusCode(await CourseService.AdminGetUserRegisteredCourses(userId, limit, offset));

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
        public async Task<IActionResult> AnonymousGetListCoursesOfCategory(string category, int? limit, int? offset)
        {
            ServiceResult res = await CourseService.AnonymousGetListCourseOfCategory(category, limit, offset);
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
        public async Task<IActionResult> AdminGetListCoursesOfCategory(string category, int? limit, int? offset)
        {
            ServiceResult res = await CourseService.AdminListCourseOfCategory(category, limit, offset);
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
        public async Task<IActionResult> UserGetListCoursesOfCategory(string category, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await CourseService.UserGetListCourseOfCategory(Guid.Parse(myUid), category, limit, offset);
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
        public async Task<IActionResult> UserGetListMarkedCourses(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await CourseService.UserGetMarkedCourses(Guid.Parse(myUid), limit, offset);
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
        public async Task<IActionResult> UserGetListCourse(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseService.UserGetListCourses(Guid.Parse(myUid), limit, offset));
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
        public async Task<IActionResult> UserGetListUserCourse(Guid userId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseService.UserGetUserCourses(Guid.Parse(myUid), userId, limit, offset));
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
        public async Task<IActionResult> UserGetListMyCourse(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseService.UserGetMyCourses(Guid.Parse(myUid), limit, offset));
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
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseService.UserGetCourseDetail(Guid.Parse(myUid), courseId));
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
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseService.UserGetMyCourseDetail(Guid.Parse(myUid), courseId));
        }


        /// <summary>
        /// Yêu cầu user get về danh sách khóa học mà mình đã đăng ký
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("my/registered-courses")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetMyRegisteredCourse()
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseService.UserGetMyRegisteredCourses(Guid.Parse(myUid)));
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
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseService.UserCreateCourse(Guid.Parse(myUid), model));
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
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseService.UserUpdateCourse(Guid.Parse(myUid), courseId, model));
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
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseService.UserDeleteCourse(Guid.Parse(myUid), courseId));
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
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseService.ChangePrivacy(Guid.Parse(myUid), model));
        }

        #endregion


        #region Coure and lesson apies

        /// <summary>
        /// Yêu cầu user thêm một bài giảng vào một khóa học
        /// </summary>
        /// <param name="model"> thông tin bài học được thêm vào khóa học </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpPost("lesson")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserAddLessonToCourse([FromBody] AddLessonToCourseModel model)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseLessonService.AddLessonToCourse(Guid.Parse(myUid), model));
        }

        /// <summary>
        /// Yêu cầu user thêm danh sách bài giảng vào khóa học
        /// </summary>
        /// <param name="model"> thông tin danh sách bài giảng cần thêm </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpPost("list-lessons")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserAddListLessonToCourse([FromBody] AddListLessonToCourseModel model)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseLessonService.AddListLessonToCourse(Guid.Parse(myUid), model));
        }


        /// <summary>
        /// Yêu cầu user xóa một bài học khỏi khóa học
        /// </summary>
        /// <param name="participantId"> Id của course-lesson cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpDelete("lesson/{participantId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserDeleteLessonFromCourse(Guid participantId)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseLessonService.DeleteLessonFromCourse(Guid.Parse(myUid), participantId));
        }

        /// <summary>
        /// Yêu cầu user xóa danh sách bài học khỏi khóa học
        /// </summary>
        /// <param name="listParticipantIds"> Danh sách id của course-lesson cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpDelete("list-lessons")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserDeleteListLessonFromCourse([FromBody] IEnumerable<Guid> listParticipantIds)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseLessonService.DeleteListLessonFromCourse(Guid.Parse(myUid), listParticipantIds));
        }


        /// <summary>
        /// Yêu cầu user cập nhật thông tin một bài học trong khóa học
        /// </summary>
        /// <param name="model"> Thông tin cập nhật course-lesson </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpPatch("lesson")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserUpdateLessonInCourse([FromBody] UpdateLessonInCourseModel model)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseLessonService.UpdateLessonInCourse(Guid.Parse(myUid), model));
        }

        /// <summary>
        /// Yêu cầu user cập nhật danh sách bài học trong khóa học
        /// </summary>
        /// <param name="listParticipantIds"> Danh sách id của course-lesson cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpPatch("list-lessons")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserUpdateListLessonInCourse([FromBody] UpdateListLessonInCourseModel model)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseLessonService.UpdateListLessonInCourse(Guid.Parse(myUid), model));
        }


        #endregion
    }
}
