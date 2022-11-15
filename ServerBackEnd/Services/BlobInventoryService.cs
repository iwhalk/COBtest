using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using SharedLibrary;
using SharedLibrary.Models;
using System.Reflection.Metadata;
using static System.Reflection.Metadata.BlobBuilder;

namespace ApiGateway.Services
{
    public class BlobInventoryService : GenericProxy, IBlobInventoryService
    {
        public BlobInventoryService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<bool>> DeleteBlobInventoryAsync(int idBlobs)
        {
            return await DeleteAsync<bool>(idBlobs, path: "BlobInventory");
        }

        public async Task<ApiResponse<List<BlobsInventory>>> GetBlobsInventoryAsync()
        {
            return await GetAsync<List<BlobsInventory>>(path: "BlobInventory");
        }

        public async Task<ApiResponse<BlobsInventory>> PostBlobInventoryAsync(BlobsInventory blobs)
        {
            return await PostAsync<BlobsInventory>(blobs, path: "BlobInventory");
        }

        public async Task<ApiResponse<BlobsInventory>> PutBlobInventoryAsync(BlobsInventory blobs)
        {
            return await PutAsync<BlobsInventory>(blobs.IdBlobs, blobs, path: "Blobs");
        }
    }
}
