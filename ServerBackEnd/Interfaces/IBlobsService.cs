using Shared.Models;
using Shared;

namespace ApiGateway.Interfaces
{
    public interface IBlobsService
    {
        Task<ApiResponse<List<Blob>>> GetBlobsAsync();
        Task<ApiResponse<Blob>> PostBlobAsync(Blob blob);
        Task<ApiResponse<Blob>> PutBlobAsync(Blob blob);
        Task<ApiResponse<bool>> DeleteBlobAsync(int idBlob);
    }
}
