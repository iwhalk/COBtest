using SharedLibrary.Models;
using SharedLibrary;

namespace Obra.Client.Interfaces
{
    public interface IProgressReportService
    {        
        Task<ProgressReport> GetProgressReportAsync(int id);
        Task<List<ProgressReport>> GetProgressReportsAsync(int? idProgressReport = null, int? idBuilding = null, int? idApartment = null, int? idArea = null, int? idElement = null, int? idSubElement = null, string? idSupervisor = null, bool includeProgressLogs = false);
        Task<ObjectAccessUser> GetObjectAccessAsync(string idSupervisor);
        Task<ProgressReport> PostProgressReportAsync(ProgressReport progressReport);
    }
}
