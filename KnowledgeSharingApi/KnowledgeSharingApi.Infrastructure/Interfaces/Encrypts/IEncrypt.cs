using KnowledgeSharingApi.Domains.Models.Dtos;
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
        /// Mã hóa đối tượng JwtTokenDto thành jwt token
        /// </summary>
        /// <param name="tokenDto"> Token cần mã hóa </param>
        /// <returns> jwt cần lấy </returns>
        /// Created: PhucTV (21/2/24)
        /// Modified: None
        string? JwtEncrypt(JwtTokenDto tokenDto);

        /// <summary>
        /// Giải mã token về đối tượng JwtTokenDto
        /// </summary>
        /// <param name="token"> chuỗi token cần giải mã </param>
        /// <returns> Mảng claim cần lấy </returns>
        /// Created: PhucTV (21/2/24)
        /// Modified: None
        JwtTokenDto? JwtDecrypt(string token, bool isValidateLifeTime);


        /// <summary>
        /// Mã hóa danh sách claim thành jwt token
        /// </summary>
        /// <param name="claims"> Mảng Claim cần mã hóa </param>
        /// <returns> jwt cần lấy </returns>
        /// Created: PhucTV (21/2/24)
        /// Modified: None
        string? JwtEncrypt(List<Claim> claims);

        /// <summary>
        /// Giải mã token về Danh sách claim
        /// </summary>
        /// <param name="token"> chuỗi token cần giải mã </param>
        /// <returns> Mảng claim cần lấy </returns>
        /// Created: PhucTV (21/2/24)
        /// Modified: None
        List<Claim>? JwtDecryptToListClaims(string token, bool isValidateLifeTime);


        /// <summary>
        /// Giải mã token về Danh sách claim
        /// </summary>
        /// <param name="token"> chuỗi token cần giải mã </param>
        /// <returns> CLaimPrincipal cần lấy </returns>
        /// Created: PhucTV (21/2/24)
        /// Modified: None
        ClaimsPrincipal? JwtDecryptToClaimsPrincipal(string token, bool isValidateLifeTime);


        /// <summary>
        /// Hàm băm một username + password để tạo thành hash password cho tài khoản
        /// </summary>
        /// <param name="username"> tên tài khoản </param>
        /// <param name="password"> mật khẩu </param>
        /// <returns> hashpassword sau khi băm </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        string HashPassword(string username, string password);


        /// <summary>
        /// Kiem tra password hop le
        /// </summary>
        /// <param name="username"> tên tài khoản </param>
        /// <param name="password"> mật khẩu </param>
        /// <param name="hashedPassword"> Ma bam mat khau </param>
        /// <returns> true - hop le, false - khong hop le </returns>
        /// Created: PhucTV (12/3/24)
        /// Modified: None
        bool VerifyPassword(string username, string password, string hashedPassword);

    }
}
