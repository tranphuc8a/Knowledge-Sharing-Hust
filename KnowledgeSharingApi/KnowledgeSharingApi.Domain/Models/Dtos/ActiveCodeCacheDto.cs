using KnowledgeSharingApi.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Dtos
{
    public class ActiveCodeCacheDto
    {
        public string Email { get; set; } = string.Empty;

        public string ActiveCode { get; set; } = string.Empty;

        public EVerifyCodeType VerifyCodeType { get; set; }

        public DateTime Expired { get; set; }
    }
}
