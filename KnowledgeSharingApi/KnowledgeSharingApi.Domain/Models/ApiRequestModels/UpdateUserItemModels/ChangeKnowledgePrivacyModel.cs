using KnowledgeSharingApi.Domains.Annotations.Converters;
using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels.UpdateUserItemModels
{
    public class ChangeKnowledgePrivacyModel
    {
        [CustomRequiredValidator(ErrorMessage = ViConstantResource.ID_EMPTY)]
        public Guid? KnowledgeId { get; set; }

        [PrivacyConverter]
        [CustomRequiredValidator(ErrorMessage = ViConstantResource.PRIVACY_EMPTY)]
        public EPrivacy? Privacy { get; set; }
    }
}
