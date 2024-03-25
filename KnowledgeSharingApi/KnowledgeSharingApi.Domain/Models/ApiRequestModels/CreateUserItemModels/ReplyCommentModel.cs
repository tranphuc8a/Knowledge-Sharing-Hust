using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels.CreateUserItemModels
{
    public class ReplyCommentModel
    {
        [CustomRequiredValidator(ErrorMessage = ViConstantResource.ID_EMPTY)]
        public Guid? ReplyId { get; set; }

        [CustomRequiredValidator(ErrorMessage = ViConstantResource.CONTENT_EMPTY)]
        public string? Content { get; set; }
    }
}
