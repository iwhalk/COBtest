using SharedLibrary.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface IInventoryService
    {
        Task<List<Inventory?>> GetInventoryAsync();
        Task<Inventory?> CreateInventoryAsync(Inventory inventory);
    }
}

