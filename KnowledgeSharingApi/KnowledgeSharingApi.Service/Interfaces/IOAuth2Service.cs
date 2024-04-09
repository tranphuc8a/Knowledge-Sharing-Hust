using KnowledgeSharingApi.Domains.Models.ApiRequestModels.AuthenticationModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Interfaces
{
    public interface IOAuth2Service
    {
        /// <summary>
        /// Thuc hien dang nhap bang google token
        /// </summary>
        /// <param name="googleToken"> token cua google cap </param>
        /// <returns></returns>
        /// Created: PhucTV (10/4/24)
        /// Modified: None
        Task<ServiceResult> LoginByGoogle(string googleToken);

        /// <summary>
        /// Thuc hien yeu cau dang ky bang google token
        /// </summary>
        /// <param name="googleToken"> token cua google cap </param>
        /// <returns> Active Code </returns>
        /// Created: PhucTV (10/4/24)
        /// Modified: None
        Task<ServiceResult> RequestSigninByGoogle(string googleToken);

        /// <summary>
        /// Thuc hien tao moi tai khoan dua tren active code
        /// </summary>
        /// <param name="googleToken"> token cua google cap </param>
        /// <returns></returns>
        /// Created: PhucTV (10/4/24)
        /// Modified: None
        Task<ServiceResult> RegsiterUserByActiveCode(ActiveCodeRegisterModel model);
    }
}
