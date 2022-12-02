using SharedLibrary.Models;

namespace Client.Interfaces
{
    public interface IBuildingsService
    {
        Task<List<Building>> GetBuildingsAsync();
        Task<Building> PostBuildingAsync(Building building);
    }
}
