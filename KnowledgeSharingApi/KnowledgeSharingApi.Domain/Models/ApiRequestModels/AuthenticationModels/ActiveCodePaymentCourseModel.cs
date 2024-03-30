using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels.AuthenticationModels
{
    public class ActiveCodePaymentCourseModel : ActiveCodeModel
    {
        [CustomRequiredValidator(ErrorMessage = ViConstantResource.ID_EMPTY)]
        public Guid? CourseId { get; set; }
    }
}
