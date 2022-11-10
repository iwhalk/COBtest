using SharedLibrary.Models;

namespace FrontEnd.Interfaces
{
    public interface IAreaService
    {
        Task<List<Area>> GetAreaAsync();
        Task<Area> PostAreaAsync(Area area);
    }
}
