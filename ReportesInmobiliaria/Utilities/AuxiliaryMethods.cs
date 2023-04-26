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
        public Image DateImage(Image imageToAddDate)
        {
            try
            {                


                FontFamily fontFamily;

                string currentDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                float WatermarkPadding = 12f;
                float fontSize = 12f;
                string WatermarkFont = "DejaVu Serif";                

                if (!SystemFonts.TryGet(WatermarkFont, out fontFamily))
                    throw new Exception($"Couldn't find font {WatermarkFont}");

                if (imageToAddDate.Width > 100 && imageToAddDate.Width < 500)
                    fontSize = 12f;
                else if (imageToAddDate.Width >= 500 && imageToAddDate.Width < 1100)
                    fontSize = 20f;
                else if (imageToAddDate.Width >= 1100)
                    fontSize = 64f;
                else
                    fontSize = 8f;
                    var font = fontFamily.CreateFont(fontSize, FontStyle.Bold);
                var options = new TextOptions(font)
                {                    
                    Dpi = 72,
                    KerningMode = KerningMode.Normal
                };

                var rect = TextMeasurer.Measure(currentDate, options);

                //Image imagen = Image.Load(imgageStream);

                imageToAddDate.Mutate(x => x.DrawText(
                    currentDate,
                    font,
                    new Color(Rgba32.ParseHex("#FFE23F")),
                    new PointF(imageToAddDate.Width - rect.Width - WatermarkPadding,
                            imageToAddDate.Height - rect.Height - WatermarkPadding)));

                return imageToAddDate;
            }
            catch
            {
                throw;
            }

        }
    }
}