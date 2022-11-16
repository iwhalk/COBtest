using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IBlobsService
    {
        Task<ApiResponse<List<Blob>>> GetBlobsAsync();
        Task<ApiResponse<Blob>> PostBlobAsync(Blob blob, IFormFile file);
        Task<ApiResponse<Blob>> PutBlobAsync(Blob blob);
        Task<ApiResponse<bool>> DeleteBlobAsync(int idBlob);
    }
}
