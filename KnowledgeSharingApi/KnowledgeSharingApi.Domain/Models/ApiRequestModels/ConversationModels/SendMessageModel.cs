using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels.ConversationModels
{
    public class SendMessageModel
    {
        [CustomRequiredValidator(ErrorMessage = ViConstantResource.ID_EMPTY)]
        public Guid ConversationId { get; set; }

        [CustomRequiredValidator(ErrorMessage = ViConstantResource.CONTENT_EMPTY)]
        public string? Content { get; set; }
    }
}
