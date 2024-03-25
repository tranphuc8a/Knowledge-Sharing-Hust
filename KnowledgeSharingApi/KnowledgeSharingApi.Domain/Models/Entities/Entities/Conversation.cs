using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Entities
{
    public class Conversation : Entity
    {
        [Key]
        public Guid ConversationId { get; set; } = Guid.Empty;
        public string ConversationName { get; set; } = string.Empty;
        public string? Thumbnail { get; set; }
        protected override Conversation Init()
        {
            return new Conversation();
        }
    }
}
