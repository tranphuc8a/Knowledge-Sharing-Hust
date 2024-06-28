using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces.Authentications
{
    public interface IGoogleRecaptchaService
    {

        /// <summary>
        /// Ham kiem tra response cua recaptcha gui toi tu client
        /// </summary>
        /// <param name="recaptchaResponse"> response can kiem tra </param>
        /// <returns> none </returns>
        /// <exception> auto throw bad request exception with invalid captcha response </exception>
        /// Created PhucTV 31/5/24
        /// Modified None
        Task CheckRecaptchaResponse(string? recaptchaResponse);
    }
}
