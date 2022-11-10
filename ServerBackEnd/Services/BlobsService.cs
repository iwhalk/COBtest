using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using Shared;
using Shared.Models;

namespace ApiGateway.Services
{
    public class BlobsService : GenericProxy, IBlobsService
    {
        public BlobsService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<bool>> DeleteBlobAsync(int idBlob)
        {
            return await DeleteAsync<bool>(idBlob, path: "Blob");
        }

        public async Task<ApiResponse<List<Blob>>> GetBlobsAsync()
        {
            return await GetAsync<List<Blob>>(path: "Blobs");
        }

        public async Task<ApiResponse<Blob>> PostBlobAsync(Blob blob)
        {
            return await PostAsync<Blob>(blob, path: "Blobs");
        }

        public async Task<ApiResponse<Blob>> PutBlobAsync(Blob blob)
        {
            return await PutAsync<Blob>(blob.IdBlobs, blob, path: "Blobs");
        }
    }
}
