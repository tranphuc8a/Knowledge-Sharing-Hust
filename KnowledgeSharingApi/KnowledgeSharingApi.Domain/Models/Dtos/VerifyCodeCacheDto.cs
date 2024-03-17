using KnowledgeSharingApi.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Dtos
{
    public class VerifyCodeCacheDto
    {
        // Code để quyết định xem có được xử lý hay không, guid, trả về qua Api
        public string AccessCode { get; set; } = String.Empty;

        // Code xác thực, trả về qua email
        public string Code { get; set; } = String.Empty;

        // Thời hạn của code xác thực (thường đề +3 phút)
        public DateTime Expired { get; set; }

        // Loại code xác thực (đang đăng ký mới hay đang quên mật khẩu)
        public EVerifyCodeType VerifyCodeType { get; set; }

        // Số lần thử code xác thực còn lại (ban đầu đặt = 3 lần)
        public int RemainAttemptNumber { get; set; }

    }
}
