using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Tables
{
    [Table("Message")]
    public class Message : Entity
    {
        public Guid MessageId { get; set; }
        public Guid UserConservationId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime Time { get; set; }
        public Guid? ReplyId { get; set; }
        public bool IsEdited { get; set; }

        protected override Message Init()
        {
            return new Message();
        }
    }
}
