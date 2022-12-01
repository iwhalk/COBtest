using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IAreasService
    {
        Task<ApiResponse<Area>> GetAreaAsync(int id);
        Task<ApiResponse<List<Area>>> GetAreasAsync();
        Task<ApiResponse<Area>> PostAreaAsync(Area area);
    }
}
