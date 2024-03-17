using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Dtos
{
    public class JwtTokenDto
    {
        public string Username { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public string Role { get; set; } = string.Empty;

        public Guid SessionId { get; set; }
    }
}
