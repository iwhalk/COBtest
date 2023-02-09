using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Services
{
    public class ImageService : GenericProxy, IImageService
    {
        public ImageService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {
            
        }
        public async Task<ApiResponse<bool>> PostImageAsync(ImageData imageData)
        {
            return await PostAsync<bool>(imageData, path: "MixImages");
        }
    }
}


