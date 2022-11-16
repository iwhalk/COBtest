using SharedLibrary.Models;

namespace FrontEnd.Interfaces
{
    public interface IBlobsInventoryService
    {
        Task<List<BlobsInventory>> GetBlobsInventoryAsync();
        Task<BlobsInventory> PostBlobsInventoryAsync(BlobsInventory blobsInventory);
    }
}
