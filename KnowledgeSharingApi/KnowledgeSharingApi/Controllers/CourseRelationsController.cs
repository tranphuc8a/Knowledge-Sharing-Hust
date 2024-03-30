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
    ) : ControllerBase
    {
        protected readonly ICourseRelationService CourseRelationService = courseRelationService;
        protected readonly ICoursePaymentService CoursePaymentService = coursePaymentService;
        protected readonly IUserRepository UserRepository = userRepository;

        protected virtual IActionResult StatusCode(ServiceResult result)
        {
            return StatusCode((int)result.StatusCode, new ApiResponse(result));
        }

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
            Guid myUid = GetCurrentUserId();
            User? myUser = await UserRepository.Get(myUid);
            if (myUser?.Email != email) throw invalidEmail;
        }

        /// <summary>
        /// Lấy về user id của current User
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        protected virtual Guid GetCurrentUserId()
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return Guid.Parse(myUid);
        }

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
        public async Task<IActionResult> AdminGetListCourseRegisters(Guid courseId, int? limit, int? offset)
            => StatusCode(await CourseRelationService.AdminGetCourseRegisters(courseId, limit, offset));


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
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetRegisters(Guid courseId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseRelationService.UserGetRegisters(Guid.Parse(myUid), courseId, limit, offset));
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
        public async Task<IActionResult> UserGetCourseInvites(Guid courseId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseRelationService.UserGetCourseInvites(Guid.Parse(myUid), courseId, limit, offset));
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
        public async Task<IActionResult> UserGetCourseRequests(Guid courseId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseRelationService.UserGetCourseRequests(Guid.Parse(myUid), courseId, limit, offset));
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
        public async Task<IActionResult> UserGetMyCourseInvites(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseRelationService.UserGetMyCourseInvites(Guid.Parse(myUid), limit, offset));
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
        public async Task<IActionResult> UserGetMyCourseRequests(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseRelationService.UserGetMyCourseRequests(Guid.Parse(myUid), limit, offset));
        }


        #endregion


        #region Payment course apies

        /// <summary>
        /// Yêu cầu chủ khóa học lấy về danh sách payment của khóa học
        /// Owner
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("payments/{courseId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetCoursePayments(Guid courseId, int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseRelationService.UserGetCoursePayments(Guid.Parse(myUid), courseId, limit, offset));
        }

        /// <summary>
        /// Yêu cầu user lấy về danh sách các payment mà mình đã thanh toán
        /// Owner của payments
        /// </summary>
        /// <param name="limit"> Số lượng phần tử trang </param>
        /// <param name="offset"> Độ lệch trang </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("payments/my")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetMyPayments(int? limit, int? offset)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseRelationService.UserGetMyPayments(Guid.Parse(myUid), limit, offset));
        }

        /// <summary>
        /// Yêu cầu user lấy về chi tiết một payment
        /// Owner của payment hoặc owner của course của payment
        /// </summary>
        /// <param name="paymentId"> id của payment cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("registers/{paymentId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetPayment(Guid paymentId)
        {
            string myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier) ?? string.Empty;
            return StatusCode(await CourseRelationService.UserGetPayment(Guid.Parse(myUid), paymentId));
        }


        /// <summary>
        /// Yêu cầu user gửi mã xác minh thanh toán khóa học về email
        /// </summary>
        /// <param name="email"> email của user cần thanh toán </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("send-payment-verification-code/{email}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> SendPaymentVerificationCode(string email)
        {
            await CheckMatchEmail(email);
            return StatusCode(await CoursePaymentService.SendVerifyCode(email));
        }


        /// <summary>
        /// Yêu cầu user kiểm tra mã xác minh thanh toán khóa học
        /// </summary>
        /// <param name="model"> thông tin mã xác minh cần kiểm tra </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("check-payment-verification-code")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> CheckPaymentVerificationCode([FromBody] VerifyCodeModel model)
        {
            await CheckMatchEmail(model.Email ?? string.Empty);
            return StatusCode(await CoursePaymentService.CheckVerifyCode(model));
        }


        /// <summary>
        /// Tiến hành thanh toán và đăng ký khóa học
        /// </summary>
        /// <param name="model"> thông tin thanh toán và đăng ký khóa học </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("pay-course")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> CheckPaymentVerificationCode([FromBody] ActiveCodePaymentCourseModel model)
        {
            await CheckMatchEmail(model.Email ?? string.Empty);
            return StatusCode(await CoursePaymentService.Action(model));
        }

        #endregion

        #region User operation apies

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
            return StatusCode(await CourseRelationService.UserRegisterCourse(GetCurrentUserId(), courseId));
        }

        /// <summary>
        /// Yêu cầu user hủy đăng ký tham gia một khóa học miễn phí
        /// Guest
        /// </summary>
        /// <param name="courseId"> id của khóa học cần hủy đăng ký </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpPost("register/{courseId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserUnregisterCourse(Guid courseId)
        {
            return StatusCode(await CourseRelationService.UserUnregisterCourse(GetCurrentUserId(), courseId));
        }

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
            return StatusCode(await CourseRelationService.UserRequestCourse(GetCurrentUserId(), courseId));
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
            return StatusCode(await CourseRelationService.UserDeleteCourseRequest(GetCurrentUserId(), requestId));
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
            return StatusCode(await CourseRelationService.UserConfirmCourseRequest(GetCurrentUserId(), requestId, isAccept));
        }

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
            return StatusCode(await CourseRelationService.UserInviteUserToCourse(GetCurrentUserId(), courseId, userId));
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
        public async Task<IActionResult> UserInviteListUserToCourse(Guid courseId, [FromBody] IEnumerable<Guid> listUserIds)
        {
            return StatusCode(await CourseRelationService.UserInviteListUserToCourse(GetCurrentUserId(), courseId, listUserIds));
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
            return StatusCode(await CourseRelationService.UserConfirmInvite(GetCurrentUserId(), inviteId, isAccept));
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
            return StatusCode(await CourseRelationService.UserDeleteInvite(GetCurrentUserId(), inviteId));
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
            return StatusCode(await CourseRelationService.UserDeleteRegister(GetCurrentUserId(), registerId));
        }



        #endregion
    }
}
