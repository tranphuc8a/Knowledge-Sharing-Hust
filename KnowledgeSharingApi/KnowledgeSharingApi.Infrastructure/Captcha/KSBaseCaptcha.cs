using System.Security.Claims;
using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Captcha;

namespace KnowledgeSharingApi.Infrastructures.Captcha
{
    public abstract class KSBaseCaptcha(IEncrypt jwtService) : ICaptcha, IDisposable
    {
        protected readonly IEncrypt JwtService = jwtService;
        protected readonly Random random = new(DateTime.UtcNow.Second);
        protected readonly string CAPTCHA_KEY = "Captcha";
        protected readonly string EXPIRED_KEY = "Expired";

        public virtual string? DecodeCaptcha(string token)
        {
            // Step 1. Decode token
            List<Claim>? listClaims = JwtService.JwtDecryptToListClaims(token, false)?.ToList();
            if (listClaims == null) return null;

            // Step 2. Lấy ra trường EXPIRED
            Claim? expired = listClaims.FirstOrDefault(claim => claim.Type == EXPIRED_KEY);
            if (expired == null) return null;

            // Step 3. Kiểm tra chưa hết hạn
            bool isDateTime = DateTime.TryParse(expired.Value, out DateTime dateTime);
            if (isDateTime)
            {
                if (dateTime < DateTime.UtcNow) return null;

                // Step 4. Lấy ra trường CAPTCHA
                Claim? captcha = listClaims.FirstOrDefault(claim => claim.Type == CAPTCHA_KEY);
                if (captcha == null) return null;

                // Trả về giá trị của trường
                return captcha.Value;
            }
            return null;
        }

        public virtual void Dispose() { }

        public virtual string? EncodeCaptcha(string captcha, int expiredInMinutes = 1)
        {
            List<Claim> claims = [
                new Claim(CAPTCHA_KEY, captcha),
                new Claim(EXPIRED_KEY, DateTime.UtcNow.AddMinutes(expiredInMinutes).ToString())
            ];
            return JwtService.JwtEncrypt(claims);
        }

        public virtual byte[] GenerateBase64ImageCaptcha(string captcha)
        {
            throw new NotImplementedException();
        }

        public virtual string GenerateRandomCaptcha(int length = 6)
        {
            string numbers = "0123456789";
            char[] chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = numbers[random.Next(numbers.Length)];
            }

            return new string(chars);
        }
    }
}
