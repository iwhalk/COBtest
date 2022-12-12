using SharedLibrary.Models;
using SharedLibrary;

namespace Obra.Client.Interfaces
{
    public interface IProgressLogsService
    {
        Task<List<ProgressLog>> GetProgressLogsAsync(int? idProgressLog = null, int? idProgressReport = null, int? idStatus = null, string? idSupervisor = null);
        Task<ProgressLog> PostProgressLogAsync(ProgressLog progressLog);
    }
}
