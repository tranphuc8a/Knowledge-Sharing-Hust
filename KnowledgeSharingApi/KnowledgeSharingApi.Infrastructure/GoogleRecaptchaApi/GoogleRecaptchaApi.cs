using KnowledgeSharingApi.Domains.Models.GoogleRecaptcha;
using KnowledgeSharingApi.Infrastructures.Interfaces.GoogleRecaptcha;
using KnowledgeSharingApi.Infrastructures.Request;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.GoogleRecaptchaApi
{
    public class GoogleRecaptchaApi(IConfiguration configuration) : IGoogleRecaptchaApi
    {
        protected readonly IConfiguration Configuration = configuration;
        protected readonly string Url = "https://www.google.com/recaptcha/api/siteverify";
        
        public async Task<GoogleRecaptchaResponse?> VerifyResponse(string responseToken)
        {
            if (string.IsNullOrEmpty(responseToken)) return new GoogleRecaptchaResponse()
            {
                Success = false
            };

            string? secretKey = Configuration["ReCaptchaV2ScretKey"];
            if (string.IsNullOrEmpty(secretKey)) return null;

            GoogleRecaptchaResponse? response = await new PostRequest(Url)
                .AddFormData("secret", secretKey)
                .AddFormData("response", responseToken)
                .Execute<GoogleRecaptchaResponse>();

            return response;
        }
    }
}
