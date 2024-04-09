using KnowledgeSharingApi.Domains.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.OAuth2
{
    public interface IOAuth2
    {
        /// <summary>
        /// Lay ve thong tin user theo token cua dich vu
        /// </summary>
        /// <param name="token"> token cua dich vu cung cap </param>
        /// <returns> thong tin user, hoac null neu user khong ton tai </returns>
        /// Created: PhucTV (10/4/24)
        /// Modified: None
        Task<OAuthUserDto?> GetuserByToken(string token);
    }
}
