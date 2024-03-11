using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities
{
    public class UserConservation : Entity
    {
        public Guid UserConservationId { get; set; } = Guid.Empty;
        public Guid UserId { get; set; } = Guid.Empty;
        public Guid ConservationId { get; set; } = Guid.Empty;
        public string? Nickname { get; set; }
        public DateTime Time { get; set; }
        public DateTime LastReadTime {  get; set; }
        public DateTime LastDeleteTime { get; set; }
        protected override UserConservation Init()
        {
            return new UserConservation();
        }
    }
}
