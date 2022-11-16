using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IDescriptionService
    {
        Task<ApiResponse<List<Description>>> GetDescriptionAsync();
        Task<ApiResponse<Description>> PostDescriptionAsync(Description description);
    }
}
