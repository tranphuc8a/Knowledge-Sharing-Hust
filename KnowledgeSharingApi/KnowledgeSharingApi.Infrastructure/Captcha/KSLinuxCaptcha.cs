using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;
using SkiaSharp;


namespace KnowledgeSharingApi.Infrastructures.Captcha
{
    public class CaptchaCharacterStyle
    {
        public SKTypeface FontFamily { get; set; } = SKTypeface.Default; // Font family của ký tự
        public SKColor FontColor { get; set; } // Màu của ký tự
        public int FontSize { get; set; } // Kích thước của ký tự
        public SKPoint Position { get; set; } // Vị trí của ký tự trong hình ảnh
        public float RotationAngle { get; set; } // Góc xoay của ký tự

    }
    public class KSLinuxCaptcha(IEncrypt jwtService) : KSBaseCaptcha(jwtService)
    {
        private readonly int Width = 170;
        private readonly int Height = 50;

        private readonly int MaxCharacterWidth = 20;

        private readonly CaptchaCharacterStyle[] characterStyles = new CaptchaCharacterStyle[6];

        private static readonly List<string> fontFamilies = [
            "Arial",
            "Verdana",
            "Times New Roman",
            "Georgia",
            "Courier New",
            "Comic Sans MS"
        ];

        public override byte[] GenerateBase64ImageCaptcha(string captcha)
        {
            // Random các thông số cho từng ký tự
            Random random = new();
            int characterWidth = Width / captcha.Length;

            for (int i = 0; i < captcha.Length; i++)
            {
                int deltaWidth = random.Next(-characterWidth / 4, characterWidth / 4);
                characterStyles[i] = new CaptchaCharacterStyle
                {
                    FontFamily = SKTypeface.FromFamilyName(fontFamilies[random.Next(0, fontFamilies.Count)]),
                    FontColor = new SKColor((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)),
                    FontSize = random.Next(18, 24),
                    Position = new SKPoint(i * characterWidth + characterWidth/2 + deltaWidth, random.Next(Height/3, Height - 2)),
                    RotationAngle = random.Next(-15, 15) // Random góc xoay từ -15 đến 15 độ
                };
            }

            using var surface = SKSurface.Create(new SKImageInfo(Width, Height));
            var canvas = surface.Canvas;

            // Tạo màu xám và trắng
            var grayColor = new SKColor(200, 200, 200);
            var whiteColor = SKColors.White;

            // Tạo gradient cho nền xọc gai
            var gradientColors = new SKColor[] { grayColor, whiteColor };
            var gradientShader = SKShader.CreateLinearGradient(
                new SKPoint(0, 0),
                new SKPoint(0, Height),
                gradientColors,
                null,
                SKShaderTileMode.Repeat);

            var bgPaint = new SKPaint
            {
                Shader = gradientShader,
                Style = SKPaintStyle.Fill
            };

            // Vẽ nền nền xọc gai
            canvas.DrawRect(0, 0, Width, Height, bgPaint);

            
            for (int i = 0; i < captcha.Length; i++)
            {
                var paint = new SKPaint
                {
                    Typeface = characterStyles[i].FontFamily,
                    TextSize = characterStyles[i].FontSize,
                    Color = characterStyles[i].FontColor,
                    TextAlign = SKTextAlign.Center, // Canh giữa văn bản
                    TextScaleX = 1, // Không scale văn bản
                    TextSkewX = 0, // Không bóp văn bản
                    IsAntialias = true // Hiệu ứng chống nhòe
                };

                canvas.Save();
                canvas.RotateDegrees(characterStyles[i].RotationAngle, characterStyles[i].Position.X + MaxCharacterWidth / 2, characterStyles[i].Position.Y + characterStyles[i].FontSize / 2);
                canvas.DrawText(captcha[i].ToString(), characterStyles[i].Position.X, characterStyles[i].Position.Y, paint);
                canvas.Restore();
            }

            // Chuyển hình ảnh thành mảng byte
            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
        }

    }
}
