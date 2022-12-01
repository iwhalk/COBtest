using SharedLibrary;
using SharedLibrary.Models;

namespace ApiGateway.Interfaces
{
    public interface IProgressReportService
    {
        Task<ApiResponse<List<ProgressReport>>> GetProgressReportsAsync();
        Task<ApiResponse<ProgressReport>> PostProgressReportAsync(ProgressReport progressReport);
    }
}
