using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using ApiGateway.Models;
using SharedLibrary;
using SharedLibrary.Models;

namespace ApiGateway.Services
{
    public class BlobsService : GenericProxy, IBlobsService
    {
        public BlobsService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<List<Blob>>> GetBlobsAsync()
        {
            return await GetAsync<List<Blob>>(path: "Blobs");
        }

        public async Task<ApiResponse<Blob>> PostBlobAsync(Blob blob)
        {
            return await PostAsync<Blob>(blob, path: "Blobs");
        }
    }
}
