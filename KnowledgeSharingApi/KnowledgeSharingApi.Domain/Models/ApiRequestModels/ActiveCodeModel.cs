using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels
{
    public class ActiveCodeModel
    {
        [EmailValidator(ErrorMessage = ViConstantResource.EMAIL_FORMAT)]
        [CustomRequiredValidator(ErrorMessage = ViConstantResource.EMAIL_EMPTY)]
        public string Email { get; set; } = string.Empty;

        [CustomRequiredValidator(ErrorMessage = ViConstantResource.ACCESS_CODE_EMPTY)]
        public string ActiveCode { get; set; } = string.Empty;
    }
}
