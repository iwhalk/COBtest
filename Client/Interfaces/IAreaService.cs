using SharedLibrary.Models;

namespace Client.Interfaces
{
    public interface IAreaService
    {
        Task<List<Area>> GetAreaAsync();
        Task<List<AreaService>> GetAreaServicesAsync();
        Task<Area> PostAreaAsync(Area area);
    }
}
