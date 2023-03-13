using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IStatusService
    {
        Task<ApiResponse<List<Status>>> GetStatusesAsync();
        Task<ApiResponse<Status>> GetStatusAsync(int idStatus);        
    }
}
