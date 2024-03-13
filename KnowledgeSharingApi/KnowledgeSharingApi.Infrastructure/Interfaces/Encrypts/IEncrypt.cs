using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts
{
    public interface IEncrypt
    {
        /// <summary>
        /// Mã hóa danh sách claim thành jwt token
        /// </summary>
        /// <param name="claims"> Mảng Claim cần mã hóa </param>
        /// <returns> jwt cần lấy </returns>
        /// Created: PhucTV (21/2/24)
        /// Modified: None
        string? JwtEncrypt(IEnumerable<Claim> claims);

        /// <summary>
        /// Giải mã token về Danh sách claim
        /// </summary>
        /// <param name="token"> chuỗi token cần giải mã </param>
        /// <returns> Mảng claim cần lấy </returns>
        /// Created: PhucTV (21/2/24)
        /// Modified: None
        IEnumerable<Claim>? JwtDecrypt(string token);


        /// <summary>
        /// Giải mã token về Danh sách claim
        /// </summary>
        /// <param name="token"> chuỗi token cần giải mã </param>
        /// <returns> CLaimPrincipal cần lấy </returns>
        /// Created: PhucTV (21/2/24)
        /// Modified: None
        ClaimsPrincipal? JwtDecryptToClaimsPrincipal(string token);


        /// <summary>
        /// Hàm băm một username + password để tạo thành hash password cho tài khoản
        /// </summary>
        /// <param name="username"> tên tài khoản </param>
        /// <param name="password"> mật khẩu </param>
        /// <returns> hashpassword sau khi băm </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        string Sha256HashPassword(string username, string password);


        /// <summary>
        /// Tùy chỉnh xem có validate theo expired của jwt không
        /// </summary>
        /// <param name="isValidateJwtTokenLifeTime"> true - có, false - không </param>
        /// Created: PhucTV (13/3/24)
        /// Modified: None
        IEncrypt SetIsValidateJwtTokenLifeTime(bool isValidateJwtTokenLifeTime);
    }
}
