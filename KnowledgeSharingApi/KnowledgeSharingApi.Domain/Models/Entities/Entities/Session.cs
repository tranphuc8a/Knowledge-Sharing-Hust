using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Entities
{
    public class Session : Entity
    {
        [Key]
        public Guid SessionId { get; set; } = Guid.Empty;
        public Guid UserId { get; set; } = Guid.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime Expired { get; set; } = DateTime.UtcNow;
        public DateTime Time { get; set; } = DateTime.UtcNow;
        public string? Place { get; set; }
        public string? Device { get; set; }

        protected override Session Init()
        {
            return new Session();
        }
    }
}
