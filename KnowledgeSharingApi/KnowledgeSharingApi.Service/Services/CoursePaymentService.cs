using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.AuthenticationModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.Caches;
using KnowledgeSharingApi.Infrastructures.Interfaces.Emails;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class CoursePaymentService : BaseVerifyByEmailService, ICoursePaymentService
    {
        protected override EVerifyCodeType VerifyCodeType { get; set; }
        protected override string EmailSubject { get; set; }
        protected readonly IUserRepository UserRepository;
        protected readonly ICourseRelationRepository CourseRelationRepository;
        protected readonly ICoursePaymentRepository CoursePaymentRepository;
        protected readonly ICourseRepository CourseRepository;

        public CoursePaymentService(
            ICache cache,
            IResourceFactory resourceFactory,
            IEmail emailService,
            IUserRepository userRepository,
            ICourseRelationRepository courseRelationRepository,
            ICoursePaymentRepository coursePaymentRepository,
            ICourseRepository courseRepository
        ) : base(cache, resourceFactory, emailService)
        {
            CourseRelationRepository = courseRelationRepository;
            CoursePaymentRepository = coursePaymentRepository;
            CourseRepository = courseRepository;
            UserRepository = userRepository;
            VerifyCodeType = EVerifyCodeType.Payment;
            EmailSubject = ResponseResource.CoursePaymentEmailSubject();
        }

        public override async Task CheckEmailIsValid(string? email)
        {
            // base
            await base.CheckEmailIsValid(email);

            // Lấy về user phải tồn tại
            User? user = await UserRepository.GetByEmail(email!) 
                ?? throw new ValidatorException(ResponseResource.NotExistUser());
        }

        protected override async Task<ServiceResult> ActionHook(ActiveCodeModel codeModel)
        {
            // codeModel phải là ActiveCodePaymentCourseModel
            if (codeModel is not ActiveCodePaymentCourseModel model)
                throw new NotMatchTypeException();

            // Kiểm tra user phải đăng ký được khóa học
            User? user = await UserRepository.GetByEmail(codeModel.Email!);
            if (user == null) return ServiceResult.BadRequest(ResponseResource.NotExistUser());

            string NotExistedCourse = ResponseResource.NotExist(ResourceFactory.GetEntityResource().Course());
            Course course = await CourseRepository.CheckExisted(model.CourseId ?? Guid.Empty, NotExistedCourse);

            await CheckCanPayCourse(user, course);

            // Thực hiện Thêm đăng ký khóa học cho user
            int res = await CoursePaymentRepository.UserPayCourse(user.UserId, course.UserItemId);
            if (res <= 0)
                return ServiceResult.ServerError(ResponseResource.ServerError());

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success());
        }

        
        /// <summary>
        /// Kiểm tra xem user có thể thanh toán khóa học course hay không
        /// </summary>
        /// <param name="user"> user kiểm tra </param>
        /// <param name="course"> khóa học kiểm tra </param>
        /// <returns></returns>
        /// Created: PhucTV (30/3/24)
        /// Modified: None
        protected virtual async Task CheckCanPayCourse(User user, Course course)
        {
            // Khóa học không được phải là của user
            if (course.UserId == user.UserId)
                throw new ValidatorException("Đây là khóa học của bạn");

            // Khóa học phải public
            if (course.Privacy != EPrivacy.Public)
                throw new ValidatorException("Bạn không có quyền đăng ký khóa học này");

            // Khóa học phải tính phí
            if (course.IsFree && course.Fee <= 0)
                throw new ValidatorException("Đây là khóa học miễn phí");

            // User đang không join khóa học này
            ViewCourseRegister? courseRegister = await CourseRepository.GetViewCourseRegister(user.UserId, course.UserItemId);
            if (courseRegister != null)
                throw new ValidatorException("Bạn đã đăng ký khóa học này rồi");

            // Số dư còn lại >= Phí tham gia khóa học (bỏ qua)
        }


        protected override string EmailContent(string verifyCode)
        {
            return ResponseResource.CoursePaymentEmailContent(verifyCode);
        }
    }
}
