using SharedLibrary.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface IAreasService
    {
        Task<List<Area>?> GetAreasAsync();
        Task<Area?> CreateAreaAsync(Area area);
        Task<bool> UpdateAreaAsync(Area area);
        Task<bool> DeleteAreaAsync(int id);
    }
}