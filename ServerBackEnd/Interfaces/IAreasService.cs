using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IAreasService
    {
        Task<ApiResponse<List<Area>>> GetAreasAsync();
        Task<ApiResponse<Area>> PostAreaAsync(Area area);
    }
}
