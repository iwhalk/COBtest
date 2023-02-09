using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IImageService
    {
        Task<bool> MixImage(ImageData imageData);
    }
}
