using SharedLibrary;
using SharedLibrary.Models;

namespace Client.Interfaces
{
    public interface IBlobsService
    {
        Task<List<Blob>> GetBlobsAsync();
        Task<Blob> PostBlobAsync(Blob blob);
    }
}
