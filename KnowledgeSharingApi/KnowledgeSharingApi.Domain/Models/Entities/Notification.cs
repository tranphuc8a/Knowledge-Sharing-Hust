using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities
{
    public class Notification : Entity
    {
        public Guid NotificationId { get; set; } = Guid.Empty;
        public Guid UserId { get; set; } = Guid.Empty;
        public string? Thumbnail { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Content { get; set; } = String.Empty;
        public string ReferenceLink { get; set; } = String.Empty;
        public DateTime Time { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;

        protected override Notification Init()
        {
            return new Notification();
        }
    }
}
