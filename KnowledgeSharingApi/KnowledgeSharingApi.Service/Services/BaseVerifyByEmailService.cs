using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.AuthenticationModels;
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
        #region Attributes
        protected readonly ICache cache = cache;
        protected readonly IResourceFactory resourceFactory = resourceFactory;
        protected readonly IResponseResource responseResource = resourceFactory.GetResponseResource();
        protected readonly IEmail emailService = emailService;
        protected readonly Random random = new();
        protected readonly int VerifyCodeExpiredInMinutes = 3;
        protected readonly int MaxNumberAttempts = 3;
        protected readonly int ActiveCodeExpiredInMinutes = 15;

        protected abstract EVerifyCodeType VerifyCodeType { get; set; }
        protected abstract string EmailSubject { get; set; }
        protected abstract string EmailContent(string verifyCode);
        #endregion

        #region Cache operations with Verify Email
        protected virtual string BoundKey(string email, EProcedureStep step)
        {
            return $"Verify Email - {VerifyCodeType} - {step} - {email}";
        }

        protected virtual void CacheSetStepOne(string email, VerifyCodeCacheDto model)
        {
            string boundKey = BoundKey(email, EProcedureStep.Step1);
            cache.Set<VerifyCodeCacheDto>(boundKey, model);
        }

        protected virtual void CacheSetStepTwo(string email, ActiveCodeCacheDto model)
        {
            string boundKey = BoundKey(email, EProcedureStep.Step2);
            cache.Set<ActiveCodeCacheDto>(boundKey, model);
        }

        protected virtual VerifyCodeCacheDto? CacheGetStepOne(string email)
        {
            string boundKey = BoundKey(email, EProcedureStep.Step1);
            return cache.Get<VerifyCodeCacheDto>(boundKey);
        }

        protected virtual ActiveCodeCacheDto? CacheGetStepTwo(string email)
        {
            string boundKey = BoundKey(email, EProcedureStep.Step2);
            return cache.Get<ActiveCodeCacheDto>(boundKey);
        }
        #endregion


        public virtual Task CheckEmailIsValid(string? email)
        {
            if (String.IsNullOrEmpty(email))
            {
                throw new ValidatorException(ViConstantResource.EMAIL_EMPTY);
            }
            if (!PropertyValidator.CheckFormatEmail(email))
            {
                throw new ValidatorException(ViConstantResource.EMAIL_FORMAT);
            }
            // Check more by override
            return Task.CompletedTask;
        }


        public virtual async Task<ServiceResult> SendVerifyCode(string? email)
        {
            // Step 1. Kiểm tra email phù hợp với action muốn thực hiện
            await CheckEmailIsValid(email);

            // Step 2. Kiểm tra xem code đã tồn tại trong cache chưa, Nếu có rồi phải đợi code hết hạn mới được
            VerifyCodeCacheDto? beforeCode = CacheGetStepOne(email!);
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
            VerifyCodeCacheDto codeModel = new()
            {
                AccessCode = accessCode,
                Code = random.Next(0, 999999).ToString("D6"),
                Expired = DateTime.Now.AddMinutes(VerifyCodeExpiredInMinutes),
                VerifyCodeType = VerifyCodeType,
                RemainAttemptNumber = MaxNumberAttempts
            };

            // Step 5. Lưu code vào cache hoặc thay thế code cũ nếu đã có
            CacheSetStepOne(email!, codeModel);

            // Step 6. Gửi code qua email, trả về thành công
            await emailService.SendAsync(
                    toEmail: email!,
                    subject: EmailSubject,
                    content: EmailContent(codeModel.Code));
            return ServiceResult.Success(responseResource.SendEmailSuccess(), String.Empty, new
            {
                AccessCode = accessCode
            });
        }


        public virtual Task<ServiceResult> CheckVerifyCode(VerifyCodeModel model)
        {
            Task<ServiceResult> expiredCode = Task.FromResult(
                ServiceResult.BadRequest(responseResource.InvalidVerifyCode())
            );

            // Step 1. Lấy codeModel với key là email từ cache (phải khác null)
            VerifyCodeCacheDto? codeModel = CacheGetStepOne(model.Email!);
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
                CacheSetStepOne(model.Email!, codeModel);
                return Task.FromResult(
                        ServiceResult.BadRequest(responseResource.WrongVerifyCode(codeModel.RemainAttemptNumber))
                    );
            }

            // Step 6. Thành công, sinh và trả về Active Code
            string ActiveCode = Guid.NewGuid().ToString();
            ActiveCodeCacheDto activeCodeCacheDto = new()
            {
                Email = model.Email!,
                Expired = DateTime.Now.AddMinutes(ActiveCodeExpiredInMinutes),
                ActiveCode = ActiveCode,
                VerifyCodeType = VerifyCodeType
            };
            CacheSetStepTwo(model.Email!, activeCodeCacheDto);

            return Task.FromResult(ServiceResult.Success(responseResource.VerifyCodeSuccess(), string.Empty, new
            {
                ActiveCode
            }));
        }
        

        protected virtual Task CheckActiveCode(ActiveCodeModel model)
        {
            ValidatorException invalidActiveCode = new(responseResource.InvalidVerifyCode());

            // Step 1. Kiểm tra tồn tại trong Cache
            ActiveCodeCacheDto dto = CacheGetStepTwo(model.Email!)
                ?? throw invalidActiveCode;

            // Step 2. Kiểm tra đúng Type
            if (dto.VerifyCodeType != VerifyCodeType) throw invalidActiveCode;

            // Step 3. Kiểm tra chưa expired
            if (DateTime.Now >= dto.Expired) throw invalidActiveCode;

            // Step 4. Kiểm tra đúng Active Code
            if (model.ActiveCode != dto.ActiveCode) throw invalidActiveCode;

            return Task.CompletedTask;
        }


        public abstract Task<ServiceResult> Action(ActiveCodeModel codeModel);
        
    }
}
