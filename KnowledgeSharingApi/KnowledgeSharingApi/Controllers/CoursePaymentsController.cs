using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.AuthenticationModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces;
using KnowledgeSharingApi.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KnowledgeSharingApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CoursePaymentsController(
        ICoursePaymentService coursePaymentService,
        IUserRepository userRepository
    ) : BaseController
    {
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


        #region Payment payments gets

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
        [HttpGet("course/{courseId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetCoursePayments(Guid courseId, int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await CoursePaymentService.UserGetCoursePayments(GetCurrentUserIdStrictly(), courseId, pagination));
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
        [HttpGet("my")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetMyPayments(int? limit, int? offset, string? order, string? filter)
        {
            PaginationDto pagination = new(limit, offset, ParseOrder(order), ParseFilter(filter));
            return StatusCode(await CoursePaymentService.UserGetMyPayments(GetCurrentUserIdStrictly(), pagination));
        }

        /// <summary>
        /// Yêu cầu user lấy về chi tiết một payment
        /// Owner của payment hoặc owner của course của payment
        /// </summary>
        /// <param name="paymentId"> id của payment cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (29/3/24)
        /// Modified: None
        [HttpGet("{paymentId}")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> UserGetPayment(Guid paymentId)
        {
            return StatusCode(await CoursePaymentService.UserGetPayment(GetCurrentUserIdStrictly(), paymentId));
        }

        #endregion

        #region Payment courses

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
        [HttpPost("check-payment-verification-code")]
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
        [HttpPost("pay-course")]
        [CustomAuthorization(Roles: "User, Admin")]
        public async Task<IActionResult> CheckPaymentVerificationCode([FromBody] ActiveCodePaymentCourseModel model)
        {
            await CheckMatchEmail(model.Email ?? string.Empty);
            return StatusCode(await CoursePaymentService.Action(model));
        }

        #endregion
    }
}
