using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CourseLessonModels;
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
    public class CourseLessonsController(
        ICourseLessonService courseLessonService    
    ) : ControllerBase
    {
        protected readonly ICourseLessonService CourseLessonService = courseLessonService;

        protected virtual IActionResult StatusCode(ServiceResult result)
        {
            return StatusCode((int)result.StatusCode, new ApiResponse(result));
        }


        #region Get apies
        /// <summary>
        /// Yêu cầu user lấy về danh sách các bài học đang có trong khóa học
        /// Owner, Member
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <param name="limit"> Kích thước trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (3/4/24)
        /// Modified: None
        [HttpGet("{courseId}")]
        [CustomAuthorization("Roles: Admin, User")]
        public async Task<IActionResult> UserGetListCourseParticipants(Guid courseId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseLessonService.UserGetListCourseParticipants(Guid.Parse(myUid), courseId, limit, offset));
        }

        /// <summary>
        /// Yêu cầu Admin lấy về danh sách các bài học đang có trong khóa học
        /// Owner, Member
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <param name="limit"> Kích thước trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (3/4/24)
        /// Modified: None
        [HttpGet("admin/{courseId}")]
        [CustomAuthorization("Roles: Admin")]
        public async Task<IActionResult> AdminGetListCourseParticipants(Guid courseId, int? limit, int? offset)
        {
            return StatusCode(await CourseLessonService.AdminGetListCourseParticipants(courseId, limit, offset));
        }

        #endregion


        #region Create Apies

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

        #endregion

        #region Delete apies

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

        #endregion

        #region Update apies

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
        /// <param name="model"> Thông tin cập nhật danh sách khóa học trong bài học </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpPatch("list-lessons")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserUpdateListLessonInCourse([FromBody] IEnumerable<UpdateLessonInCourseModel> model)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseLessonService.UpdateListLessonInCourse(Guid.Parse(myUid), model));
        }

        /// <summary>
        /// Yêu cầu user cập nhật thứ tự các bài học trong khóa học
        /// </summary>
        /// <param name="listParticipantIds"> Danh sách id của course-lesson theo thứ tự cần sắp xếp </param>
        /// <returns></returns>
        /// Created: PhucTV (01/04/24)
        /// Modified: None
        [HttpPatch("update-offsets")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserUpdateOffsetOfListLessonInCourse([FromBody] Guid[] listParticipantIds)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseLessonService.UpdateOffsetOfListLessonInCourse(Guid.Parse(myUid), listParticipantIds));
        }

        #endregion
    }
}
