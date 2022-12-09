using SharedLibrary.Models;
using SharedLibrary;

namespace Obra.Client.Interfaces
{
    public interface IProgressReportService
    {
        Task<List<AparmentProgress>?> GetProgresReportViewAsync(int? id);
        Task<ProgressReport> GetProgressReportAsync(int id);
        Task<List<ProgressReport>> GetProgressReportsAsync(int? idProgressReport = null, int? idBuilding = null, int? idAparment = null, int? idArea = null, int? idElemnet = null, int? idSubElement = null, string? idSupervisor = null);
        Task<ProgressReport> PostProgressReportAsync(ProgressReport progressReport);
    }
}
