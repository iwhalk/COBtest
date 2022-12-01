using SharedLibrary.Models;
using SharedLibrary;

namespace Client.Interfaces
{
    public interface IProgressLogsService
    {
        Task<List<ProgressLog>> GetProgressLogsAsync();
        Task<ProgressLog> PostProgressLogAsync(ProgressLog progressLog);
    }
}
