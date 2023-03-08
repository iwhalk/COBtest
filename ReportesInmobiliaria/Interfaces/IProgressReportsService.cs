using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IProgressReportsService
    {
        Task<ProgressReport> GetProgressReportAsync(int idProgressReport);
        Task<List<ProgressReport>?> GetProgressReportsAsync(int? idProgressReport, int? idBuilding, int? idApartment, int? idArea, int? idElement, int? idSubElement, string? idSupervisor, bool includeProgressLogs);
        Task<ObjectAccessUser?> GetObjectsAccessAsync(string idSupervisor);
        Task<int> GetIdBuildingAssigned(string idSupervisor);
        Task<ProgressReport?> CreateProgressReportAsync(ProgressReport progressReport);
        Task<bool> UpdateProgressReportAsync(ProgressReport progressReport);
    }
}
