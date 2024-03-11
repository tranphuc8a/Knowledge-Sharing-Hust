using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities
{
    public class User : Entity
    {
        public Guid? UserId { get; set; } = Guid.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        protected override User Init()
        {
            return new User();
        }
    }
}
