using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels.CourseRelationModels
{
    public class DeleteRegisterModel
    {
        [CustomRequiredValidator(ErrorMessage = ViConstantResource.ID_EMPTY)]
        public Guid? UserId { get; set; }

        [CustomRequiredValidator(ErrorMessage = ViConstantResource.ID_EMPTY)]
        public Guid? CourseId { get; set; }
    }
}
