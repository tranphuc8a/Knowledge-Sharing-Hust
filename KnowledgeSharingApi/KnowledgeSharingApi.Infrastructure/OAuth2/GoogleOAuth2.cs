using KnowledgeSharingApi.Domains.Models.OAuth2;
using KnowledgeSharingApi.Infrastructures.Interfaces.OAuth2;
using KnowledgeSharingApi.Infrastructures.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.OAuth2
{
    public class GoogleOAuth2 : IGoogleOAuth2
    {
        protected readonly string Url = "https://www.googleapis.com/oauth2/v2/userinfo";
        
        public async Task<GoogleOAuth2User?> GetuserByToken(string token)
        {
            GoogleOAuth2User? user = await new GetRequest(Url)
                .SetBearerAuthentication(token)
                .Execute<GoogleOAuth2User>();
            return user;
        }
    }
}
