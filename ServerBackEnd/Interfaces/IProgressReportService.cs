using SharedLibrary;
using SharedLibrary.Models;

namespace ApiGateway.Interfaces
{
    public interface IProgressReportService
    {        
        Task<ApiResponse<ProgressReport>> GetProgressReportAsync(int id);
        Task<ApiResponse<List<ProgressReport>>> GetProgressReportsAsync(int? idProgressReport, int? idBuilding, int? idApartment, int? idArea, int? idElement, int? idSubElement, string? idSupervisor, bool includeProgressLogs);
        Task<ApiResponse<ObjectAccessUser>> GetObjectAccessAsync(string idSupervisor);
        Task<ApiResponse<ProgressReport>> PostProgressReportAsync(ProgressReport progressReport);
    }
}
