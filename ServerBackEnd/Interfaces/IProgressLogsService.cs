using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IProgressLogsService
    {
        Task<ApiResponse<List<ProgressLog>>> GetProgressLogsAsync();
        Task<ApiResponse<ProgressLog>> PostProgressLogAsync(ProgressLog progressLog);
    }
}
