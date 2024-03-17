using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Captcha;

namespace KnowledgeSharingApi.Infrastructures.Captcha
{
#pragma warning disable CA1416 // Validate platform compatibility
    public class KSCaptcha(IEncrypt jwtService) : ICaptcha, IDisposable
    {
        private const int Width = 170;
        private const int Height = 50;
        readonly IEncrypt JwtService = jwtService;
        readonly Random random = new(DateTime.Now.Second);
        readonly string CAPTCHA_KEY = "Captcha";
        readonly string EXPIRED_KEY = "Expired";


        // For Drawing
        readonly Font[] fonts = [
            new("Arial", 24, FontStyle.Bold),
            new("Courier New", 22, FontStyle.Bold),
            new("Calibri", 20, FontStyle.Bold),
            new("Tahoma", 24, FontStyle.Italic | FontStyle.Bold)
        ];

        readonly List<Brush> brushes = [Brushes.Gray, Brushes.Red, Brushes.Orange, Brushes.Yellow, Brushes.Green, Brushes.Blue, Brushes.Violet];
        readonly StringFormat stringFormat = new() { LineAlignment = StringAlignment.Center };

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
                if (dateTime < DateTime.Now) return null;

                // Step 4. Lấy ra trường CAPTCHA
                Claim? captcha = listClaims.FirstOrDefault(claim => claim.Type == CAPTCHA_KEY);
                if (captcha == null) return null;

                // Trả về giá trị của trường
                return captcha.Value;
            }
            return null;
        }

        public void Dispose()
        {
            // Cleanup Fonts (they are disposable)
            foreach (Font font in fonts)
            {
                font.Dispose();
            }
        }

        public virtual string? EncodeCaptcha(string captcha, int expiredInMinutes = 1)
        {
            List<Claim> claims = [
                new Claim(CAPTCHA_KEY, captcha),
                new Claim(EXPIRED_KEY, DateTime.Now.AddMinutes(expiredInMinutes).ToString())
            ];
            return JwtService.JwtEncrypt(claims);
        }


        public virtual byte[] GenerateBase64ImageCaptcha(string captcha)
        {
            byte[]? byteArray = null;

            int subWidth = Width / captcha.Length;
            int deltaWidthRange = subWidth / 3;
            int deltaHeightRange = Height / 4;

            using (Bitmap bmp = new(Width, Height))
            {
                using (Graphics graphic = Graphics.FromImage(bmp))
                {
                    using (HatchBrush hb = new(HatchStyle.DarkUpwardDiagonal, Color.Silver, Color.White))
                    {
                        graphic.FillRectangle(hb, 0, 0, bmp.Width, bmp.Height);
                    }

                    for (int i = 0; i < captcha.Length; i++)
                    {
                        PointF point = new(
                            Math.Min(Width, Math.Max(0, i * subWidth + random.Next(-deltaWidthRange, deltaWidthRange))),
                            Height / 2 + random.Next(-deltaHeightRange, deltaHeightRange)
                        );
                        Font font = fonts[random.Next(0, fonts.Length - 1)];
                        Brush brush = brushes[random.Next(0, brushes.Count - 1)];
                        graphic.DrawString(captcha.Substring(i, 1), font, brush, point, stringFormat);
                    }
                }
                using MemoryStream stream = new();
                bmp.Save(stream, ImageFormat.Png);
                byteArray = stream.ToArray();
            }

            return byteArray;
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
#pragma warning restore CA1416 // Validate platform compatibility
}
