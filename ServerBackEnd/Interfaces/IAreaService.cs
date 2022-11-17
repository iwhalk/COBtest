using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IAreaService
    {
        Task<ApiResponse<List<Area>>> GetAreaAsync();
        Task<ApiResponse<List<AreaService>>> GetAreaServicesAsync();
        Task<ApiResponse<Area>> PostAreaAsync(Area area);
    }
}
