using FrontEnd.Models;
using SharedLibrary.Models;

namespace FrontEnd.Interfaces
{
    public interface IBlobService
    {
        Task<List<Blob>> GetBlobsAsync();
        Task<Blob> PostBlobAsync(Blob Blob);
        Task<Blob> PostBlobAsync(BlobFile BlobFile);
    }
}
