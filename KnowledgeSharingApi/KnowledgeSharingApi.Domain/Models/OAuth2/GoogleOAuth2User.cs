using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.OAuth2
{
    public class GoogleOAuth2User
    {
        public string id { get; set; } = string.Empty;

        public string email { get; set; } = string.Empty;

        public bool verified_email { get; set; } = false;

        public string name { get; set; } = string.Empty;

        public string given_name { get; set; } = string.Empty;

        public string family_name { get; set; } = string.Empty;

        public string? picture { get; set; }

        public string? locale { get; set; }
    }
}
