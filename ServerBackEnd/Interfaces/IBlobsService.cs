using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IBlobsService
    {
        Task<ApiResponse<byte[]>> GetBlobImage(int id);
        Task<ApiResponse<Blob>> GetBlobAsync(int id);
        Task<ApiResponse<List<Blob>>> GetBlobsAsync(int? id);
        Task<ApiResponse<Blob>> PostBlobAsync(Blob blob, IFormFile file);
        Task<ApiResponse<bool>> DeleteBlobAsync(int idBlob);
    }
}
