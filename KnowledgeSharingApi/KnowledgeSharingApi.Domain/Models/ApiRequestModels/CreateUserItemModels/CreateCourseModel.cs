using KnowledgeSharingApi.Domains.Annotations.Converters;
using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels.CreateUserItemModels
{
    public class CreateCourseModel : CreateKnowledgeModel
    {
        [PrivacyConverter]
        [CustomRequiredValidator(ErrorMessage = ViConstantResource.EMPLOYEE_CODE_EMPTY)]
        public EPrivacy? Privacy { get; set; }

        [CustomRequiredValidator(ErrorMessage = ViConstantResource.COURSE_INTRODUCTION_EMPTY)]
        public string? Introduction { get; set; }

        [CustomRequiredValidator(ErrorMessage = ViConstantResource.COURSE_FEE_EMPTY)]
        public int? Fee { get; set; }

        [CustomRequiredValidator(ErrorMessage = ViConstantResource.ESTIMATION_EMPTY)]
        public int? EstimateTimeInMinutes { get; set; }
    }
}
