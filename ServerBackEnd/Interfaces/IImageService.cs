using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IImageService
    {
        Task<ApiResponse<bool>> PostImageAsync(ImageData imageData);
    }
}

