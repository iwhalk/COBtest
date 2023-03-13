using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IStatusService
    {
        Task<List<Status>?> GetStatusesAsync();
        Task<Status?> GetStatusAsync(int idStatus);
    }
}
