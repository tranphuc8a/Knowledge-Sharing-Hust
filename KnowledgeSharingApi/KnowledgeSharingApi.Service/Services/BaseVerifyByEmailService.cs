using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using KnowledgeSharingApi.Infrastructures.Interfaces.Caches;
using KnowledgeSharingApi.Infrastructures.Interfaces.Emails;
using KnowledgeSharingApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace KnowledgeSharingApi.Services.Services
{
    public abstract class BaseVerifyByEmailService(
            ICache cache,
            IResourceFactory resourceFactory,
            IEmail emailService
        ) : IVerifyByEmailService
    {
        protected readonly ICache cache = cache;
        protected readonly IResourceFactory resourceFactory = resourceFactory;
        protected readonly IResponseResource responseResource = resourceFactory.GetResponseResource();
        protected readonly IEmail emailService = emailService;
        protected readonly Random random = new();
        protected readonly EVerifyCodeType VerifyCodeType = EVerifyCodeType.ForgotPassword;
        protected readonly int VerifyCodeExpiredInMinutes = 3;
        protected readonly int MaxNumberAttempts = 3;

        public abstract Task<ServiceResult> Action(VerifyCodeModel codeModel);

        public virtual Task CheckEmailIsValid(string? email)
        {
            if (String.IsNullOrEmpty(email) || !PropertyValidator.CheckFormatEmail(email))
            {
                throw new ValidatorException(ViConstantResource.EMAIL_FORMAT);
            }
            // Check more by override
            return Task.CompletedTask;
        }


        public virtual Task<ServiceResult> CheckVerifyCode(VerifyCodeModel model)
        {
            Task<ServiceResult> expiredCode = Task.FromResult(
                ServiceResult.BadRequest(responseResource.InvalidVerifyCode())
            );

            // Step 1. Lấy codeModel với key là email từ cache (phải khác null)
            VerifyCodeDto? codeModel = cache.Get<VerifyCodeDto>(model.Email!);
            if (codeModel == null) return expiredCode;

            // Step 2. Kiểm tra Access Code phải khớp
            if (model.AccessCode != codeModel.AccessCode) 
                return Task.FromResult(ServiceResult.BadRequest(responseResource.InvalidAccessCode()));

            // Step 3. Kiểm tra số lần attempt còn lại phải > 0
            if (codeModel.RemainAttemptNumber <= 0) return expiredCode;

            // Step 4. Kiểm tra code chưa hết hạn, nếu hết hạn, xóa khỏi cache
            if (codeModel.Expired < DateTime.Now)
            {
                cache.Remove(model.Email!);
                return expiredCode;
            }
            if (codeModel.VerifyCodeType != VerifyCodeType) return expiredCode;

            // Step 5. Kiểm tra email và code phải match với nhau,
            // nếu không khớp, giảm số lần attemt, ghi lại cache
            if (model.Code != codeModel.Code)
            {
                codeModel.RemainAttemptNumber--;
                cache.Set<VerifyCodeDto>(model.Email!, codeModel);
                return Task.FromResult(
                        ServiceResult.BadRequest(responseResource.WrongVerifyCode(codeModel.RemainAttemptNumber))
                    );
            }

            // Step 6. Trả về Verify thành công
            return Task.FromResult(ServiceResult.Success(responseResource.VerifyCodeSuccess()));
        }


        public virtual async Task<ServiceResult> SendVerifyCode(string? email)
        {
            // Step 1. Kiểm tra email phù hợp với action muốn thực hiện
            await CheckEmailIsValid(email);
            

            // Step 2. Kiểm tra xem code đã tồn tại trong cache chưa, Nếu có rồi phải đợi code hết hạn mới được
            VerifyCodeDto? beforeCode = cache.Get<VerifyCodeDto>(email!);
            if (beforeCode != null && beforeCode.VerifyCodeType == VerifyCodeType)
            {
                if (beforeCode.Expired > DateTime.Now)
                {
                    // Tính số giây còn phải đợi nữa
                    int waitInSeconds = (int)beforeCode.Expired.Subtract(DateTime.Now).TotalSeconds;
                    return ServiceResult.BadRequest(responseResource.WaitInSecond(waitInSeconds));
                }
            }

            // Step 3. Tạo code random, expired
            string accessCode = Guid.NewGuid().ToString();
            VerifyCodeDto codeModel = new()
            {
                AccessCode = accessCode,
                Code = random.Next(0, 999999).ToString("D6"),
                Expired = DateTime.Now.AddMinutes(VerifyCodeExpiredInMinutes),
                VerifyCodeType = EVerifyCodeType.ForgotPassword,
                RemainAttemptNumber = MaxNumberAttempts
            };

            // Step 5. Lưu code vào cache hoặc thay thế code cũ nếu đã có
            cache.Set<VerifyCodeDto>(email!, codeModel);

            // Step 6. Gửi code qua email, trả về thành công
            emailService.SetToEmail(email!)
                        .SetSubject(responseResource.ForgotPasswordEmailSubject())
                        .SetContent(responseResource.ForgotPasswordEmailContent(codeModel.Code))
                        .Send();
            return ServiceResult.Success(responseResource.SendEmailSuccess(), String.Empty, new
            {
                AccessCode = accessCode
            });
        }
    }
}
