using SharedLibrary.Models;

namespace FrontEnd.Interfaces
{
    public interface IAreasService
    {
        Task<List<Area>> GetAreaAsync();
        Task<List<AreaService>> GetAreaServicesAsync();
        Task<Area> PostAreaAsync(Area area);
    }
}
