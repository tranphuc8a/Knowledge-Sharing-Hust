using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.GoogleRecaptcha
{
    public class GoogleRecaptchaResponse
    {

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTime? ChallengeTs { get; set; } 
        // Timestamp của challenge. Lưu ý: Dạng này có thể null

        [JsonProperty("hostname")]
        public string? Hostname { get; set; } 
        // Hostname của site gửi kiểm tra

        [JsonProperty("error-codes")]
        public List<string>? ErrorCodes { get; set; } 
        // Danh sách các mã lỗi, nếu có

        [JsonProperty("score")]
        public decimal? Score { get; set; } 
        // Điểm số mà reCAPTCHA v3 trả về, chỉ tồn tại khi sử dụng reCAPTCHA v3

        [JsonProperty("action")]
        public string? Action { get; set; }
        // Hành động mà người dùng thực hiện, chỉ tồn tại khi sử dụng reCAPTCHA v3
    }
}
