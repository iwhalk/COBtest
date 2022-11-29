using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IInventoryService
    {
        Task<List<Inventory?>> GetInventoryAsync();
        Task<Inventory?> CreateInventoryAsync(Inventory inventory);
    }
}

