using SharedLibrary.Models;

namespace Obra.Client.Interfaces
{
    public interface IStatusesService
    {
        Task<List<Status>> GetStatusesAsync();
        Task<Status> GetStatusAync(int idStatus);
    }
}
