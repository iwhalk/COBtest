using Shared.Models;
using Shared;

namespace ApiGateway.Interfaces
{
    public interface IInventoryService
    {
        Task<ApiResponse<List<Inventory>>> GetInventoryAsync();
        Task<ApiResponse<Inventory>> PostInventoryAsync(Inventory inventory);
    }
}
