using Shared;
using Shared.Models;

namespace ApiGateway.Interfaces
{
    public interface IBlobInventoryService
    {
        Task<ApiResponse<List<BlobsInventory>>> GetBlobsInventoryAsync();
        Task<ApiResponse<BlobsInventory>> PostBlobInventoryAsync(BlobsInventory blobs);
        Task<ApiResponse<BlobsInventory>> PutBlobInventoryAsync(BlobsInventory blobs);
        Task<ApiResponse<bool>> DeleteBlobInventoryAsync(int idBlobs);
    }
}
