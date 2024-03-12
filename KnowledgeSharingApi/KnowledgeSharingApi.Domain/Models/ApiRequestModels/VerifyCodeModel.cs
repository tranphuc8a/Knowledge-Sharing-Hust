using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels
{
    public class VerifyCodeModel
    {
        [CustomRequiredValidator(ErrorMessage = ViConstantResource.EMAIL_EMPTY)]
        [EmailValidator(ErrorMessage = ViConstantResource.EMAIL_FORMAT)]
        public string? Email { get; set; }

        [CustomRequiredValidator(ErrorMessage = ViConstantResource.VERIFY_CODE_EMPTY)]
        public string? Code { get; set; }

        [CustomRequiredValidator(ErrorMessage = ViConstantResource.ACCESS_CODE_EMPTY)]
        public string? AccessCode { get; set; }

    }
}
