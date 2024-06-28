using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;

namespace KnowledgeSharingApi.Infrastructures.Captcha
{
#pragma warning disable CA1416 // Validate platform compatibility
    public class KSWindowCaptcha(IEncrypt jwtService) : KSBaseCaptcha(jwtService)
    {
        private const int Width = 170;
        private const int Height = 50;

        // For Drawing
        readonly Font[] fonts = [
            new("Arial", 24, FontStyle.Bold),
            new("Courier New", 22, FontStyle.Bold),
            new("Calibri", 20, FontStyle.Bold),
            new("Tahoma", 24, FontStyle.Italic | FontStyle.Bold)
        ];

        readonly List<Brush> brushes = [Brushes.Gray, Brushes.Red, Brushes.Orange, Brushes.Yellow, Brushes.Green, Brushes.Blue, Brushes.Violet];
        readonly StringFormat stringFormat = new() { LineAlignment = StringAlignment.Center };

        public override void Dispose()
        {
            // Cleanup Fonts (they are disposable)
            foreach (Font font in fonts)
            {
                font.Dispose();
            }
        }

        public override byte[] GenerateBase64ImageCaptcha(string captcha)
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
    }
#pragma warning restore CA1416 // Validate platform compatibility
}