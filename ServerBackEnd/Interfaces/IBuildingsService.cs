using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IBuildingsService
    {
        Task<ApiResponse<List<Building>>> GetBuildingsAsync();
        Task<ApiResponse<Building>> PostBuildingAsync(Building building);
    }
}
