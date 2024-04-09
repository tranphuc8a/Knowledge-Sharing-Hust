using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.OAuth2
{
    public interface IGoogleOAuth2
    {
        /// <summary>
        /// Lay ve thong tin user theo token cua google
        /// </summary>
        /// <param name="token"> token ma google cung cap </param>
        /// <returns> thong tin user, hoac null neu user khong ton tai </returns>
        /// Created: PhucTV (10/4/24)
        /// Modified: None
        Task<GoogleOAuth2User?> GetuserByToken(string token);
    }
}
