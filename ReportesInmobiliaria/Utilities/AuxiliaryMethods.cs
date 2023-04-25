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
using Microsoft.Extensions.Options;

namespace ReportesObra.Utilities
{
    public class AuxiliaryMethods
    {
        public Image DateImage(MemoryStream imgageStream)
        {
            try
            {                


                FontFamily fontFamily;

                string currentDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                float WatermarkPadding = 12f;
                string WatermarkFont = "DejaVu Serif";                

                if (!SystemFonts.TryGet(WatermarkFont, out fontFamily))
                    throw new Exception($"Couldn't find font {WatermarkFont}");

                var font = fontFamily.CreateFont(12, FontStyle.Bold);

                var options = new TextOptions(font)
                {
                    Dpi = 72,
                    KerningMode = KerningMode.Normal
                };

                var rect = TextMeasurer.Measure(currentDate, options);

                IImageInfo imageInfo = Image.Identify(imgageStream);
                imgageStream.Position = 0;

                Image imagen = Image.Load(imgageStream);

                imagen.Mutate(x => x.DrawText(
                    currentDate,
                    font,
                    new Color(Rgba32.ParseHex("#FFE23F")),
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