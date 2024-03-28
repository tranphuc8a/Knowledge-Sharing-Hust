﻿using KnowledgeSharingApi.Domains.Models.ApiRequestModels.CreateUserItemModels;
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
    ) : ControllerBase
    {
        public ILessonService LessonService;

        protected virtual IActionResult StatusCode(ServiceResult result)
        {
            return StatusCode((int)result.StatusCode, new ApiResponse(result));
        }

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
        public async Task<IActionResult> AnonymousGetList(int? limit, int? offset)
            => StatusCode(await LessonService.AnonymousGetPosts(limit, offset));

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
        public async Task<IActionResult> AnonymousGetUserList(Guid userId, int? limit, int? offset)
            => StatusCode(await LessonService.AnonymousGetUserPosts(userId, limit, offset));

        /// <summary>
        /// Yêu cầu lấy về chi tiết bài giảng
        /// </summary>
        /// <param name="lessonId"> id của question cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
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
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("admin")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListLessons(int? limit, int? offset)
            => StatusCode(await LessonService.AdminGetPosts(limit, offset));

        /// <summary>
        /// Yêu cầu admin lấy về danh sách bài giảng của một user
        /// </summary>
        /// <param name="userId"> id của người cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Users/admin/lessons/{userId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListUserLessons(Guid userId, int? limit, int? offset)
            => StatusCode(await LessonService.AdminGetUserPosts(userId, limit, offset));

        /// <summary>
        /// Yêu cầu admin lấy về chi tiết bài giảng
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
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
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Courses/admin/lessons/{courseId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListCourseLessons(Guid courseId, int? limit, int? offset)
            => StatusCode(await LessonService.AdminGetListPostsOfCourse(courseId, limit, offset));

        /// <summary>
        /// Yêu cầu admin xóa một bài giảng cụ thể
        /// </summary>
        /// <param name="lessonId"> id của bài giảng </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
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
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListLesson(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await LessonService.UserGetPosts(Guid.Parse(myUid), limit, offset));
        }

        /// <summary>
        /// Yêu cầu user get về danh sách bài giảng của user khác
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <param name="userId"> id của user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("api/v1/user/lessons/{userId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListUserLesson(Guid userId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await LessonService.UserGetUserPosts(Guid.Parse(myUid), userId, limit, offset));
        }

        /// <summary>
        /// Yêu cầu user get về danh sách bài giảng của chính mình
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("my")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListMyLesson(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await LessonService.UserGetMyPosts(Guid.Parse(myUid), limit, offset));
        }

        /// <summary>
        /// Yêu cầu user get về chi tiết một bài giảng
        /// </summary>
        /// <param name="lessonId"> id của bài giảng cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("{lessonId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetDetailLesson(Guid lessonId)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await LessonService.UserGetPostDetail(Guid.Parse(myUid), lessonId));
        }

        /// <summary>
        /// Yêu cầu user get về chi tiết bài giảng của mình
        /// </summary>
        /// <param name="lessonId"> id của bài giảng cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("my/{lessonId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetDetailMyLesson(Guid lessonId)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await LessonService.UserGetMyPostDetail(Guid.Parse(myUid), lessonId));
        }

        /// <summary>
        /// Yêu cầu user get về danh sách bài giảng của một khóa học
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Courses/lessons/{courseId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListCourseLesson(Guid courseId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await LessonService.UserGetListPostsOfCourse(Guid.Parse(myUid), courseId, limit, offset));
        }

        /// <summary>
        /// Yêu cầu user get về danh sách bài giảng của khóa học của mình
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Courses/my/lessons/{courseId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListMyCourseLesson(Guid courseId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await LessonService.UserGetListPostsOfMyCourse(Guid.Parse(myUid), courseId, limit, offset));
        }

        /// <summary>
        /// Yêu cầu user get về danh sách khóa học của một bài giảng
        /// </summary>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <param name="lessonId"> id của bài giảng cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpGet("my/courses/{lessonId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserGetListMyCoursesOfALesson(Guid lessonId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await LessonService.UserGetListCourseOfLesson(Guid.Parse(myUid), lessonId, limit, offset));
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
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Categories/anonymous/lessons/{category}")]
        [AllowAnonymous]
        public async Task<IActionResult> AnonymousGetListPostsOfCategory(string category, int? limit, int? offset)
        {
            ServiceResult res = await LessonService.AnonymousGetListPostsOfCategory(category, limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu admin lấy về danh sách bài giảng của một category
        /// </summary>
        /// <param name="category"> Tên loại cần lấy </param>
        /// <param name="limit"> Số lượng bài giảng cần lấy </param>
        /// <param name="offset"> Độ lệch bài giảng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Categories/admin/lessons/{category}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListPostsOfCategory(string category, int? limit, int? offset)
        {
            ServiceResult res = await LessonService.AdminGetListPostsOfCategory(category, limit, offset);
            return StatusCode(res);
        }


        /// <summary>
        /// Xử lý yêu cầu user lấy về danh sách bài giảng của một category
        /// </summary>
        /// <param name="category"> Tên loại cần lấy </param>
        /// <param name="limit"> Số lượng bài giảng cần lấy </param>
        /// <param name="offset"> Độ lệch bài giảng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Categories/lessons/{category}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetListPostsOfCategory(string category, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await LessonService.UserGetListPostsOfCategory(Guid.Parse(myUid), category, limit, offset);
            return StatusCode(res);
        }

        /// <summary>
        /// Xử lý yêu cầu user lấy về danh sách bài giảng mà mình đã mark
        /// </summary>
        /// <param name="limit"> Số lượng bài đăng cần lấy </param>
        /// <param name="offset"> Độ lệch bài đăng đầu tiên </param>
        /// <returns></returns>
        /// Created: PhucTV (25/3/24)
        /// Modified: None
        [HttpGet("/api/v1/Marks/my/lessons")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetListMarkedLessons(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            ServiceResult res = await LessonService.UserGetMyMarkedPosts(Guid.Parse(myUid), limit, offset);
            return StatusCode(res);
        }

        #endregion

        #region User operations


        /// <summary>
        /// Yêu cầu user tạo mới một bài giảng
        /// </summary>
        /// <param name="model"> thông tin bài giảng mới được tạo ra </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpPost]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserAddNewLesson([FromForm] CreateLessonModel model)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await LessonService.UserCreatePost(Guid.Parse(myUid), model));
        }


        /// <summary>
        /// Yêu cầu user cập nhật bài giảng
        /// </summary>
        /// <param name="lessonId"> id của bài giảng được cập nhật </param>
        /// <param name="model"> thông tin bài giảng mới được cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpPatch("{lessonId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserUpdateLesson(Guid lessonId, [FromForm] UpdateLessonModel model)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await LessonService.UserUpdatePost(Guid.Parse(myUid), lessonId, model));
        }


        /// <summary>
        /// Yêu cầu user xóa bài giảng
        /// </summary>
        /// <param name="lessonId"> Id của bài giảng cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpDelete("{lessonId}")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserDeleteLesson(Guid lessonId)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await LessonService.UserDeletePost(Guid.Parse(myUid), lessonId));
        }


        /// <summary>
        /// Yêu cầu user cập nhật quyền riêng tư của bài giảng
        /// </summary>
        /// <param name="model"> thông tin quyền riêng tư mới cần cập nhật </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        [HttpPatch("privacy")]
        [CustomAuthorization(Roles: "Admin, User")]
        public async Task<IActionResult> UserUpdatePrivacy([FromBody] ChangeKnowledgePrivacyModel model)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await LessonService.ChangePrivacy(Guid.Parse(myUid), model));
        }

        #endregion
    }
}