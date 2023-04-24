using SharedLibrary.Models;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Drawing.Processing;
using System.Globalization;

namespace ReportesObra.Utilities
{
    public class AuxiliaryMethods
    {
        public Image DateImage(MemoryStream imgageStream)
        {
            try
            {                


                FontFamily fontFamily;


                string text = "2023-04-24";
                float WatermarkPadding = 18f;
                string WatermarkFont = "Roboto";
                float WatermarkFontSize = 64f;

                var font = fontFamily.CreateFont(WatermarkFontSize, FontStyle.Regular);

                var options = new TextOptions(font)
                {
                    Dpi = 72,
                    KerningMode = KerningMode.Normal
                };

                var rect = TextMeasurer.Measure(text, options);

                IImageInfo imageInfo = Image.Identify(imgageStream);
                imgageStream.Position = 0;

                Image imagen = Image.Load(imgageStream);

                imagen.Mutate(x => x.DrawText(
                    text,
                    font,
                    new Color(Rgba32.ParseHex("#FFFFFFEE")),
                    new PointF(imagen.Width - rect.Width - WatermarkPadding,
                            imagen.Height - rect.Height - WatermarkPadding)));

                return imagen;
            }
            catch
            {
                throw;
            }

        }
    }
}