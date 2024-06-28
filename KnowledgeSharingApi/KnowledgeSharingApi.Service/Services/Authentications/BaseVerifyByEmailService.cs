using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiRequestModels.AuthenticationModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using KnowledgeSharingApi.Infrastructures.Interfaces.Caches;
using KnowledgeSharingApi.Infrastructures.Interfaces.Emails;
using KnowledgeSharingApi.Services.Interfaces.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace KnowledgeSharingApi.Services.Services.Authentications
{
    public abstract class BaseVerifyByEmailService(
            ICache cache,
            IResourceFactory resourceFactory,
            IEmail emailService
        ) : IVerifyByEmailService
    {
        #region Attributes
        protected readonly ICache Cache = cache;
        protected readonly IResourceFactory ResourceFactory = resourceFactory;
        protected readonly IEntityResource EntityResource = resourceFactory.GetEntityResource();
        protected readonly IResponseResource ResponseResource = resourceFactory.GetResponseResource();
        protected readonly IEmail EmailService = emailService;
        protected readonly Random Random = new();
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
            Cache.Set(boundKey, model);
        }

        protected virtual void CacheSetStepTwo(string email, ActiveCodeCacheDto model)
        {
            string boundKey = BoundKey(email, EProcedureStep.Step2);
            Cache.Set(boundKey, model);
        }

        protected virtual VerifyCodeCacheDto? CacheGetStepOne(string email)
        {
            string boundKey = BoundKey(email, EProcedureStep.Step1);
            return Cache.Get<VerifyCodeCacheDto>(boundKey);
        }

        protected virtual ActiveCodeCacheDto? CacheGetStepTwo(string email)
        {
            string boundKey = BoundKey(email, EProcedureStep.Step2);
            return Cache.Get<ActiveCodeCacheDto>(boundKey);
        }

        protected virtual void CacheDeleteStepOne(string email)
        {
            string boundKey = BoundKey(email, EProcedureStep.Step1);
            Cache.Remove(boundKey);
        }

        protected virtual void CacheDeleteStepTwo(string email)
        {
            string boundKey = BoundKey(email, EProcedureStep.Step2);
            Cache.Remove(boundKey);
        }
        #endregion


        public virtual Task CheckEmailIsValid(string? email)
        {
            if (string.IsNullOrEmpty(email))
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

            // Step 2. Kiểm tra xem code đã tồn tại trong Cache chưa, Nếu có rồi phải đợi code hết hạn mới được
            VerifyCodeCacheDto? beforeCode = CacheGetStepOne(email!);
            if (beforeCode != null && beforeCode.VerifyCodeType == VerifyCodeType)
            {
                if (beforeCode.Expired > DateTime.UtcNow)
                {
                    // Tính số giây còn phải đợi nữa
                    int waitInSeconds = (int)beforeCode.Expired.Subtract(DateTime.UtcNow).TotalSeconds;
                    return ServiceResult.BadRequest(ResponseResource.WaitInSecond(waitInSeconds));
                }
            }

            // Step 3. Tạo code Random, expired
            string accessCode = Guid.NewGuid().ToString();
            VerifyCodeCacheDto codeModel = new()
            {
                AccessCode = accessCode,
                Code = Random.Next(0, 999999).ToString("D6"),
                Expired = DateTime.UtcNow.AddMinutes(VerifyCodeExpiredInMinutes),
                VerifyCodeType = VerifyCodeType,
                RemainAttemptNumber = MaxNumberAttempts
            };

            // Step 5. Lưu code vào Cache hoặc thay thế code cũ nếu đã có
            CacheSetStepOne(email!, codeModel);

            // Step 6. Gửi code qua email, trả về thành công
            await EmailService.SendAsync(
                    toEmail: email!,
                    subject: EmailSubject,
                    content: EmailContent(codeModel.Code));
            return ServiceResult.Success(ResponseResource.SendEmailSuccess(), string.Empty, new
            {
                AccessCode = accessCode
            });
        }


        public virtual Task<ServiceResult> CheckVerifyCode(VerifyCodeModel model)
        {
            Task<ServiceResult> expiredCode = Task.FromResult(
                ServiceResult.BadRequest(ResponseResource.InvalidVerifyCode())
            );

            // Step 1. Lấy codeModel với key là email từ Cache (phải khác null)
            VerifyCodeCacheDto? codeModel = CacheGetStepOne(model.Email!);
            if (codeModel == null) return expiredCode;

            // Step 2. Kiểm tra Access Code phải khớp
            if (model.AccessCode != codeModel.AccessCode)
                return Task.FromResult(ServiceResult.BadRequest(ResponseResource.InvalidAccessCode()));

            // Step 3. Kiểm tra số lần attempt còn lại phải > 0
            if (codeModel.RemainAttemptNumber <= 0) return expiredCode;

            // Step 4. Kiểm tra code chưa hết hạn, nếu hết hạn, xóa khỏi Cache
            if (codeModel.Expired < DateTime.UtcNow)
            {
                Cache.Remove(model.Email!);
                return expiredCode;
            }
            if (codeModel.VerifyCodeType != VerifyCodeType) return expiredCode;

            // Step 5. Kiểm tra email và code phải match với nhau,
            // nếu không khớp, giảm số lần attemt, ghi lại Cache
            if (model.Code != codeModel.Code)
            {
                codeModel.RemainAttemptNumber--;
                CacheSetStepOne(model.Email!, codeModel);
                return Task.FromResult(
                        ServiceResult.BadRequest(ResponseResource.WrongVerifyCode(codeModel.RemainAttemptNumber))
                    );
            }

            // Step 6. Thành công, sinh và trả về Active Code
            string ActiveCode = Guid.NewGuid().ToString();
            ActiveCodeCacheDto activeCodeCacheDto = new()
            {
                Email = model.Email!,
                Expired = DateTime.UtcNow.AddMinutes(ActiveCodeExpiredInMinutes),
                ActiveCode = ActiveCode,
                VerifyCodeType = VerifyCodeType
            };
            CacheSetStepTwo(model.Email!, activeCodeCacheDto);

            return Task.FromResult(ServiceResult.Success(ResponseResource.VerifyCodeSuccess(), string.Empty, new
            {
                ActiveCode
            }));
        }


        protected virtual Task CheckActiveCode(ActiveCodeModel model)
        {
            ValidatorException invalidActiveCode = new(ResponseResource.InvalidVerifyCode());

            // Step 1. Kiểm tra tồn tại trong Cache
            ActiveCodeCacheDto dto = CacheGetStepTwo(model.Email!)
                ?? throw invalidActiveCode;

            // Step 2. Kiểm tra đúng Type
            if (dto.VerifyCodeType != VerifyCodeType) throw invalidActiveCode;

            // Step 3. Kiểm tra chưa expired
            if (DateTime.UtcNow >= dto.Expired) throw invalidActiveCode;

            // Step 4. Kiểm tra đúng Active Code
            if (model.ActiveCode != dto.ActiveCode) throw invalidActiveCode;

            return Task.CompletedTask;
        }


        public virtual async Task<ServiceResult> Action(ActiveCodeModel codeModel)
        {
            await CheckActiveCode(codeModel);

            ServiceResult res = await ActionHook(codeModel);

            if (res.IsSuccess)
            { // XÓa cache
                string email = codeModel.Email!;
                CacheDeleteStepOne(email);
                CacheDeleteStepTwo(email);
            }

            return res;
        }


        protected abstract Task<ServiceResult> ActionHook(ActiveCodeModel codeModel);

    }
}
