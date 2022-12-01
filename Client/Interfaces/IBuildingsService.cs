using SharedLibrary.Models;

namespace Obra.Client.Interfaces
{
    public interface IBuildingsService
    {
        Task<List<Building>> GetBuildingsAsync();
        Task<Building> PostBuildingAsync(Building building);
    }
}
