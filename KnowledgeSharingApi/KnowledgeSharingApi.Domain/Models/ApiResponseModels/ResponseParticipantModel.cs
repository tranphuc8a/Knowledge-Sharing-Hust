using KnowledgeSharingApi.Domains.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class ResponseParticipantModel : Entity
    {
        public Guid UserConversationId { get; set; }

        public Guid UserId { get; set; }

        public Guid ConversationId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string? Avatar { get; set; }

        public string? Cover { get; set; }

        public string Nickname { get; set; } = string.Empty;

        public DateTime Time { get; set; }

        public DateTime LastReadTime { get; set; }



        protected override ResponseParticipantModel Init()
        {
            return new ResponseParticipantModel();
        }
    }
}
