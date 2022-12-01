using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IProgressReportsService
    {
        Task<ProgressReport> GetProgressReportAsync(int idProgressReport);
        Task<List<ProgressReport>?> GetProgressReportsAsync(int? idProgressReport, int? idBuilding, int? idAparment, int? idArea, int? idElemnet, int? idSubElement, string? idSupervisor);
        Task<ProgressReport?> CreateProgressReportsAsync(ProgressReport progressReport);
        Task<bool> UpdateProgressReportsAsync(ProgressReport progressReport);
    }
}
