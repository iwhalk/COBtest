using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IAreasService
    {
        Task<List<Area>?> GetAreasAsync();
        Task<Area?> GetAreaAsync(int id);
        Task<Area?> CreateAreaAsync(Area area);
        Task<bool> UpdateAreaAsync(Area area);
        Task<bool> DeleteAreaAsync(int id);
    }
}
