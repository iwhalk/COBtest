using Shared.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface IBlobService
    {
        Task<List<Blob>?> GetBlobAsync();
        Task<Blob?> CreateBlobAsync(Blob blob);
        Task<bool> UpdateBlobAsync(Blob blob);
        Task<bool> DeleteBlobAsync(int id);
        Task<List<BlobsInventory>?> GetBlobInventoryAsync();
        Task<BlobsInventory?> CreateBlobInventoryAsync(BlobsInventory blobsInventory);
        Task<bool> UpdateBlobInventoryAsync(BlobsInventory blobsInventory);
        Task<bool> DeleteBlobInventoryAsync(int id);
    }
}

