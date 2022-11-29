using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IAreasService
    {
        Task<List<Area>?> GetAreasAsync();
        Task<List<AreaService>?> GetAreaServicesAsync();
        Task<Area?> CreateAreaAsync(Area area);
        Task<bool> UpdateAreaAsync(Area area);
        Task<bool> DeleteAreaAsync(int id);
    }
}