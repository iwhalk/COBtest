using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IBuildingsService
    {
        Task<List<Building>?> GetBuildingsAsync();
        Task<Building?> GetBuildingAsync(int id);
        Task<Building?> CreateBuildingAsync(Building building);
        Task<bool> UpdateBuildingAsync(Building building);
        Task<bool> DeleteBuildingAsync(int id);
    }
}
