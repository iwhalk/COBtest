using Shared.Models;

namespace FrontEnd.Interfaces
{
    public interface IInventoryService
    {
        Task<List<Inventory>> GetInventoryAsync();
        Task<Inventory> PostInventoryAsync(Inventory inventory);
    }
}
