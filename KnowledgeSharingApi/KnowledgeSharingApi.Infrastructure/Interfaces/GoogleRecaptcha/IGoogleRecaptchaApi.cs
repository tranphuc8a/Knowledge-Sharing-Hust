using KnowledgeSharingApi.Domains.Models.GoogleRecaptcha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.GoogleRecaptcha
{
    public interface IGoogleRecaptchaApi
    {

        /// <summary>
        /// Thuc hien goi api de validate response token
        /// Tra ve GoogleRecaptchaResponse (Success)
        /// </summary>
        /// <param name="responseToken"> token can kiem tra </param>
        /// <returns></returns>
        /// Created PhucTV (31/5/24)
        /// Modified None
        Task<GoogleRecaptchaResponse?> VerifyResponse(string responseToken);
    }
}
