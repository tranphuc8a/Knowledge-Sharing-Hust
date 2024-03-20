using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels
{
    public class UpdateUserConversationModel
    {
        [CustomRequiredValidator(ErrorMessage = ViConstantResource.ID_EMPTY)]
        public Guid UserConversationId { get; set; }

        [CustomRequiredValidator(ErrorMessage = ViConstantResource.NAME_EMPTY)]
        public string? NickName { get; set; }
    }
}
