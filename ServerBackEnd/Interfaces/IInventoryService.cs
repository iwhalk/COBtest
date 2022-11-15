using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IInventoryService
    {
        Task<ApiResponse<List<Inventory>>> GetInventoryAsync();
        Task<ApiResponse<Inventory>> PostInventoryAsync(Inventory inventory);
    }
}
