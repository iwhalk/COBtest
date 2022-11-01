using Shared.Models;

namespace TestingFrontEnd.Interfaces
{
    public interface IInventoryService
    {
        Task<List<Inventory>> GetInventoryAsync();
        Task<Inventory> PostInventoryAsync(Inventory inventory);
    }
}
