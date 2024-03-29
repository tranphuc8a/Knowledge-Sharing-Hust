using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels.AuthenticationModels
{
    public class LoginWithCaptchaModel
    {
        [CustomRequiredValidator(ErrorMessage = ViConstantResource.USERNAME_EMPTY)]
        public string? Username { get; set; }

        [CustomRequiredValidator(ErrorMessage = ViConstantResource.PASSWORD_EMPTY)]
        public string? Password { get; set; }

        [CustomRequiredValidator(ErrorMessage = ViConstantResource.ACCESS_TOKEN_EMPTY)]
        public string? Token { get; set; }

        [CustomRequiredValidator(ErrorMessage = ViConstantResource.ACCESS_CODE_EMPTY)]
        public string? Captcha { get; set; }
    }
}
