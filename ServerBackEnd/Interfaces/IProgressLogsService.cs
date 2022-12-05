using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IProgressLogsService
    {
        Task<ApiResponse<ProgressLog>> GetProgressLogAsync(int id);
        Task<ApiResponse<List<ProgressLog>>> GetProgressLogsAsync(int? idProgressLog, int? idProgressReport, int? idStatus, string? idSupervisor);
        Task<ApiResponse<ProgressLog>> PostProgressLogAsync(ProgressLog progressLog);
    }
}
