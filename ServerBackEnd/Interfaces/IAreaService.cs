using Shared.Models;
using Shared;

namespace ApiGateway.Interfaces
{
    public interface IAreaService
    {
        Task<ApiResponse<List<Area>>> GetAreaAsync();
        Task<ApiResponse<Area>> PostAreaAsync(Area area);
    }
}
