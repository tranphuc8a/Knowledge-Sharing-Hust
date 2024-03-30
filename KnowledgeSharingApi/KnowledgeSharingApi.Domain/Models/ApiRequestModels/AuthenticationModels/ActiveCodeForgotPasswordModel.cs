using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels.AuthenticationModels
{
    public class ActiveCodeForgotPasswordModel : ActiveCodeModel
    {
        [CustomRequiredValidator(ErrorMessage = ViConstantResource.PASSWORD_EMPTY)]
        [PasswordValidator(ErrorMessage = ViConstantResource.PASSWORD_FORMAT)]
        public string? NewPassword { get; set; }

    }
}
