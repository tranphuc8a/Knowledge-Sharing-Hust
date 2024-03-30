using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiRequestModels.ConversationModels
{
    public class SendMessageSocketModel
    {
        public Guid UserConversationId { get; set; }

        public string Content { get; set; } = string.Empty;

        public Guid? ReplyId { get; set; } = null;
    }
}
