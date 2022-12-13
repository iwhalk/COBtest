using Obra.Client.Models;
using SharedLibrary;
using SharedLibrary.Models;

namespace Obra.Client.Interfaces
{
    public interface IBlobsService
    {
        Task<byte[]> GetBlobImage(int id);
        Task<Blob> GetBlobAsync(int? id);
        Task<List<Blob>> GetBlobsAsync(int? id);
        Task<Blob> PostBlobAsync(Blob blob);
        Task<Blob> PostBlobAsync(BlobFile blobFile);
    }
}
