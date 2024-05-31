using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Entities
{
    public class Notification : Entity
    {
        [Key]
        public Guid NotificationId { get; set; } = Guid.Empty;
        public Guid UserId { get; set; } = Guid.Empty;
        public string? Thumbnail { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ReferenceLink { get; set; } = string.Empty;
        public DateTime Time { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;

        protected override Notification Init()
        {
            return new Notification();
        }
    }
}
