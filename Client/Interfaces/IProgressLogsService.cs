using SharedLibrary.Models;
using SharedLibrary;

namespace Obra.Client.Interfaces
{
    public interface IProgressLogsService
    {
        Task<List<ProgressLog>> GetProgressLogsAsync();
        Task<ProgressLog> PostProgressLogAsync(ProgressLog progressLog);
    }
}
