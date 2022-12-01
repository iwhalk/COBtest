using Azure.Storage.Blobs.Models;
using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IBlobService
    {
        Task<Blob> GetBlobAsync(int? id);
        Task<List<Blob>> GetBlobsAsync(int? id);
        Task<BlobDownloadInfo>? GetBlobFileAsync(int id);
        Task<Blob?> CreateBlobAsync(IFormFile file);
        Task<bool> UpdateBlobAsync(Blob blob);
        Task<bool> DeleteBlobAsync(int id);
    }
}

