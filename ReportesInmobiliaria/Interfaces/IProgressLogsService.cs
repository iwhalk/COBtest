using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IProgressLogsService
    {
        Task<ProgressLog> GetProgressLogAsync(int idProgressLog);
        Task<List<ProgressLog>?> GetProgressLogsAsync(int? idProgressLog, int? idProgressReport, int? idStatus, string? idSupervisor);     
        Task<ProgressLog?> CreateProgressLogsAsync(ProgressLog progressLog);
        Task<bool> UpdateProgressLogsAsync(ProgressLog progressLog);
    }
}
