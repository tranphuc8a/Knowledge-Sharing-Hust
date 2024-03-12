using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels
{
    public class VerifyCodeRegisterModel : VerifyCodeModel
    {
        [CustomRequiredValidator(ErrorMessage = ViConstantResource.USERNAME_EMPTY)]
        [UsernameValidator(ErrorMessage = ViConstantResource.USERNAME_FORMAT)]
        public string? Username { get; set; }

        [CustomRequiredValidator(ErrorMessage = ViConstantResource.PASSWORD_EMPTY)]
        [PasswordValidator(ErrorMessage = ViConstantResource.PASSWORD_FORMAT)]
        public string? Password { get; set; }

        [CustomRequiredValidator(ErrorMessage = ViConstantResource.NAME_EMPTY)]
        public string? FullName { get; set; }

    }
}
