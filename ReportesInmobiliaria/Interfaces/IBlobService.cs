using Azure.Storage.Blobs.Models;
using SharedLibrary.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface IBlobService
    {
        Task<BlobDownloadInfo>? GetBlobAsync(int id);
        Task<Blob?> CreateBlobAsync(string name, IFormFile file);
        Task<bool> UpdateBlobAsync(Blob blob);
        Task<bool> DeleteBlobAsync(int id);
        Task<List<BlobsInventory>?> GetBlobInventoryAsync();
        Task<BlobsInventory?> CreateBlobInventoryAsync(BlobsInventory blobsInventory);
        Task<bool> UpdateBlobInventoryAsync(BlobsInventory blobsInventory);
        Task<bool> DeleteBlobInventoryAsync(int id);
    }
}

