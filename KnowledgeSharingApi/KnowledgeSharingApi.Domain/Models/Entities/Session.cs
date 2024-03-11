using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities
{
    public class Session : Entity
    {
        public Guid SessionId { get; set; } = Guid.Empty;
        public Guid UserId { get; set; } = Guid.Empty;
        public string RefreshToken { get; set; } = String.Empty;
        public DateTime Expired { get; set; } = DateTime.Now;
        public DateTime Time {  get; set; } = DateTime.Now;
        public string? Place { get; set; }
        public string? Device { get; set; }

        protected override Session Init()
        {
            return new Session();
        }
    }
}
