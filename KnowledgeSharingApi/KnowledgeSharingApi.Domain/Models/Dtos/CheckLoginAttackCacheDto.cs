using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Dtos
{
    public class CheckLoginAttackCacheDto
    {
        public string Username { get; set; } = string.Empty;

        public DateTime LastAccessTime { get; set; }

        public int NumberFailedAttempt { get; set; } = 0;

    }
}
