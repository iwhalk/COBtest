using SharedLibrary.Models;

namespace Obra.Client.Interfaces
{
    public interface IBuildingsService
    {
        Task<Building> GetBuildingAsync(int id);
        Task<List<Building>> GetBuildingsAsync();
        Task<Building> PostBuildingAsync(Building building);
    }
}
