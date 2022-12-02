using SharedLibrary.Models;
using SharedLibrary;

namespace Obra.Client.Interfaces
{
    public interface IProgressReportService
    {
        Task<List<ProgressReport>> GetProgressReportsAsync();
        Task<ProgressReport> PostProgressReportAsync(ProgressReport progressReport);
    }
}
