using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Captcha
{
    public interface ICaptcha
    {
        /// <summary>
        /// Sinh ra một mã captcha ngẫu nhiên với chiều dài cho trước
        /// </summary>
        /// <param name="length"> Chiều dài của mã Captcha</param>
        /// <returns> mã captcha ngẫu nhiên </returns>
        /// Created: PhucTV (01/03/24)
        /// Modified: None
        string GenerateRandomCaptcha(int length = 6);

        /// <summary>
        /// Sinh hình ảnh từ mã captcha dạng byte[]
        /// </summary>
        /// <param name="captcha"> Mã captcha cần sinh ảnh </param>
        /// <returns> Mảng byte[] dữ liệu hình ảnh </returns>
        /// Created: PhucTV (01/03/24)
        /// Modified: None
        byte[] GenerateBase64ImageCaptcha(string captcha);

        /// <summary>
        /// Mã hóa captcha thành token
        /// </summary>
        /// <param name="captcha"> Mã captcha cần mã hóa </param>
        /// <param name="expiredInMinutes"> Thời hạn captcha, đơn vị phút </param>
        /// <returns> token sau khi mã hóa </returns>
        /// Created: PhucTV (01/03/24)
        /// Modified: None
        string? EncodeCaptcha(string captcha, int expiredInMinutes = 3);

        /// <summary>
        /// Giải mã token lấy captcha
        /// </summary>
        /// <param name="token"> token cần giải mã </param>
        /// <returns> mã captcha hoặc null (nếu không hợp lệ) </returns>
        /// Created: PhucTV (01/03/24)
        /// Modified: None
        string? DecodeCaptcha(string token);

    }
}
