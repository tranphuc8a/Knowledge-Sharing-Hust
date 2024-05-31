using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Models.GoogleRecaptcha;
using KnowledgeSharingApi.Infrastructures.Interfaces.GoogleRecaptcha;
using KnowledgeSharingApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services
{
    public class GoogleRecaptchaService(IGoogleRecaptchaApi googleRecaptchaApi) : IGoogleRecaptchaService
    {
        protected readonly IGoogleRecaptchaApi GoogleRecaptchaApi = googleRecaptchaApi;

        public async Task CheckRecaptchaResponse(string? recaptchaResponse)
        {
            ResponseException badRequest = new()
            {
                StatusCode = EStatusCode.BadRequest,
                DevMessage = "Recaptcha response wrong or expired",
                UserMessage = "Mã xác minh Captcha không đúng hoặc đã hết hạn"
            };
            if (string.IsNullOrEmpty(recaptchaResponse)) throw badRequest;

            GoogleRecaptchaResponse? res = await GoogleRecaptchaApi.VerifyResponse(recaptchaResponse) ?? throw new ResponseException()
            {
                StatusCode = EStatusCode.ServerError,
                DevMessage = "Verify recaptcha error",
                UserMessage = "Lỗi hệ thống, vui lòng thử lại",
            };

            if (res.Success == false) throw badRequest;

            // Pass if success
        }
    }
}
