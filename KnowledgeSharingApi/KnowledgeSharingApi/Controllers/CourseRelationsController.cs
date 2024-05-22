using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.AuthenticationModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KnowledgeSharingApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CourseRelationsController(
        ICourseRelationService courseRelationService,
        ICoursePaymentService coursePaymentService,
        IUserRepository userRepository
    ) : BaseController
    {
        protected readonly ICourseRelationService CourseRelationService = courseRelationService;
        protected readonly ICoursePaymentService CoursePaymentService = coursePaymentService;
        protected readonly IUserRepository UserRepository = userRepository;

        #region Functionality methods

        /// <summary>
        /// Thực hiện kiểm tra email có trùng khớp với current user hay không
        /// </summary>
        /// <param name="email"> email cần kiếm tra </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        protected virtual async Task CheckMatchEmail(string email)
        {
            ValidatorException invalidEmail = new(ViConstantResource.INVALID_EMAIL);
            if (string.IsNullOrEmpty(email)) throw invalidEmail;
            Guid myUid = GetCurrentUserIdStrictly();
            User? myUser = await UserRepository.Get(myUid);
            if (myUser?.Email != email) throw invalidEmail;
        }

        #endregion

        #region Admin apies
        /// <summary>
        /// Yêu cầu admin lấy về danh sách người dùng đã đăng ký học trong một khóa học
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <param name="limit"> Số lượng </param>
        /// <param name="offset"> Độ lệch </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("admin/registers/{courseId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminGetListCourseRegisters(Guid courseId, int? limit, int? offset, string? order, string? filter)
            => StatusCode(await CourseRelationService.AdminGetCourseRegisters(courseId, 
                new PaginationDto(limit, offset, ParseOrder(order), ParseFilter(filter))));


        /// <summary>
        /// Yêu cầu admin xóa một user ra khỏi một course bất kỳ
        /// Chỉ cho phép xòa mỗi lần một register, không cho xóa hàng loạt
        /// Không cho phép xóa user join bằng payment
        /// </summary>
        /// <param name="registerId"> id của register cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpDelete("admin/registers/{registerId}")]
        [CustomAuthorization(Roles: "Admin")]
        public async Task<IActionResult> AdminDeleteUserFromCourse(Guid registerId)
            => StatusCode(await CourseRelationService.AdminDeleteUserFromCourse(registerId));


        #endregion



        #region Get APies

        /// <summary>
        /// Yêu cầu user lấy về danh sách những người đã đăng ký tham gia khóa học
        /// Owner hoặc member
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("registers/{courseId}")]
        [AllowAnonymous]
        public async Task<IActionResult> UserGetRegisters(Guid courseId, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await CourseRelationService.UserGetRegisters(GetCurrentUserId(), courseId, pagination));
        }


        /// <summary>
        /// Yêu cầu chủ khóa học lấy về danh sách invite của một khóa học
        /// Owner
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("invites/{courseId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetCourseInvites(Guid courseId, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await CourseRelationService.UserGetCourseInvites(GetCurrentUserIdStrictly(), courseId, pagination));
        }

        /// <summary>
        /// Yêu cầu chủ khóa học lấy về danh sách request của một khóa học
        /// Owner
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("requests/{courseId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetCourseRequests(Guid courseId, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await CourseRelationService.UserGetCourseRequests(GetCurrentUserIdStrictly(), courseId, pagination));
        }

        /// <summary>
        /// Yêu cầu chủ khóa học lấy về danh sách invite của một khóa học
        /// Owner
        /// </summary>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("invites/my")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetMyCourseInvites(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await CourseRelationService.UserGetMyCourseInvites(GetCurrentUserIdStrictly(), pagination));
        }

        /// <summary>
        /// Yêu cầu user học lấy về danh sách request của mình
        /// Owner
        /// </summary>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("requests/my")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetMyCourseRequests(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await CourseRelationService.UserGetMyCourseRequests(GetCurrentUserIdStrictly(), pagination));
        }


        /// <summary>
        /// Yêu cầu user lay ve quan he voi mot khoa hoc
        /// User
        /// <paramref name="courseId"/> id cua khoa hoc can lay
        /// <paramref name="isFocusCourse"/> focus vao course hay user
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (15/5/24)
        /// Modified: None
        [HttpGet("course-status/{courseId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetCourseStatus(Guid courseId, bool? isFocusCourse)
        {
            return StatusCode(await CourseRelationService.UserGetCourseRelationStatus(GetCurrentUserIdStrictly(), courseId, isFocusCourse));
        }

        /// <summary>
        /// Yêu cầu user lay ve quan he voi mot khoa hoc
        /// User
        /// <paramref name="courseId"/> id cua khoa hoc can lay
        /// <paramref name="userId"/> id user can lay
        /// <paramref name="isFocusCourse"/> focus vao course hay user
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (15/5/24)
        /// Modified: None
        [HttpGet("course-user-status/{courseId}/{userId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetUserCourseStatus(Guid courseId, Guid userId, bool? isFocusCourse)
        {
            return StatusCode(await CourseRelationService.UserGetCourseRelationStatus(GetCurrentUserIdStrictly(), userId, courseId, isFocusCourse));
        }


        #endregion


        #region User operation apies

        #region Registers

        /// <summary>
        /// Yêu cầu user đăng ký tham gia một khóa học miễn phí
        /// Guest
        /// </summary>
        /// <param name="courseId"> id của khóa học cần đăng ký </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpPost("register/{courseId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserRegisterCourse(Guid courseId)
        {
            return StatusCode(await CourseRelationService.UserRegisterCourse(GetCurrentUserIdStrictly(), courseId));
        }

        /// <summary>
        /// Yêu cầu user hủy đăng ký tham gia một khóa học
        /// Guest
        /// </summary>
        /// <param name="courseId"> id của khóa học cần hủy đăng ký </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpPost("unregister/{courseId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserUnregisterCourse(Guid courseId)
        {
            return StatusCode(await CourseRelationService.UserUnregisterCourse(GetCurrentUserIdStrictly(), courseId));
        }

        /// <summary>
        /// Yêu cầu user kick user khác khỏi khóa học hiện tại của mình
        /// Sẽ có nhiều điều kiện ràng buộc
        /// </summary>
        /// <param name="registerId"> id của phiên user đăng ký khóa học </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpDelete("register/{registerId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserDeleteRegister(Guid registerId)
        {
            return StatusCode(await CourseRelationService.UserDeleteRegister(GetCurrentUserIdStrictly(), registerId));
        }

        #endregion


        #region Request

        /// <summary>
        /// Yêu cầu user yêu cầu tham gia một khóa học
        /// Guest, khóa học có tính phí
        /// </summary>
        /// <param name="courseId"> id của khóa học cần yêu cầu </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpPost("request/{courseId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserRequestCourse(Guid courseId)
        {
            return StatusCode(await CourseRelationService.UserRequestCourse(GetCurrentUserIdStrictly(), courseId));
        }

        /// <summary>
        /// Yêu cầu user xóa một course request
        /// </summary>
        /// <param name="requestId"> id của request cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpDelete("request/{requestId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserDeleteCourseRequest(Guid requestId)
        {
            return StatusCode(await CourseRelationService.UserDeleteCourseRequest(GetCurrentUserIdStrictly(), requestId));
        }

        /// <summary>
        /// Yêu cầu user xác nhận yêu cầu tham gia khóa học
        /// </summary>
        /// <param name="requestId"> id của yêu cầu tham gia khóa học </param>
        /// <param name="isAccept"> Có đồng ý hay không </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpPost("request/confirm/{requestId}/{isAccept}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserConfirmCourseRequest(Guid requestId, bool isAccept)
        {
            return StatusCode(await CourseRelationService.UserConfirmCourseRequest(GetCurrentUserIdStrictly(), requestId, isAccept));
        }

        #endregion

        #region Invite

        /// <summary>
        /// Yêu cầu user invite một user khác tham gia khóa học
        /// </summary>
        /// <param name="courseId"> id của khóa học được invite vào </param>
        /// <param name="userId"> id của user cần invitew </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpPost("invite/{courseId}/{userId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserInviteUserToCourse(Guid courseId, Guid userId)
        {
            return StatusCode(await CourseRelationService.UserInviteUserToCourse(GetCurrentUserIdStrictly(), courseId, userId));
        }

        /// <summary>
        /// Yêu cầu user invite một list user khác tham gia khóa học
        /// </summary>
        /// <param name="courseId"> id của khóa học được invite vào </param>
        /// <param name="listUserIds"> danh sách id của những user cần invitew </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpPost("invite-list/{courseId}/{userId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserInviteListUserToCourse(Guid courseId, [FromBody] List<Guid> listUserIds)
        {
            return StatusCode(await CourseRelationService.UserInviteListUserToCourse(GetCurrentUserIdStrictly(), courseId, listUserIds));
        }

        /// <summary>
        /// Yêu cầu user xác nhận lời mời tham gia khóa học
        /// </summary>
        /// <param name="inviteId"> id của lời mời cần xác nhận </param>
        /// <param name="isAccept"> có đồng ý hay không </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpPost("invite/confirm/{inviteId}/{isAccept}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserConfirmInvite(Guid inviteId, bool isAccept)
        {
            return StatusCode(await CourseRelationService.UserConfirmCourseInvite(GetCurrentUserIdStrictly(), inviteId, isAccept));
        }

        /// <summary>
        /// Yêu cầu user xóa lời mời tham gia khóa học
        /// </summary>
        /// <param name="inviteId"> id của lời mời cần xóa </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpDelete("invite/{inviteId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserDeleteInvite(Guid inviteId)
        {
            return StatusCode(await CourseRelationService.UserDeleteCourseInvite(GetCurrentUserIdStrictly(), inviteId));
        }

        #endregion

        #endregion
    }
}
