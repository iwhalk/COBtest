using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IBlobsService
    {
        Task<ApiResponse<List<Blob>>> GetBlobsAsync();
        Task<ApiResponse<Blob>> PostBlobAsync(Blob blob);
    }
}
