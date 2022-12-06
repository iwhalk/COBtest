using SharedLibrary;
using SharedLibrary.Models;

namespace Obra.Client.Interfaces
{
    public interface IBlobsService
    {
        Task<Blob> GetBlobAsync(int? id);
        Task<List<Blob>> GetBlobsAsync(int? id);
        Task<Blob> PostBlobAsync(Blob blob);
    }
}
