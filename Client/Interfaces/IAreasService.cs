using SharedLibrary.Models;
using SharedLibrary;

namespace Client.Interfaces
{
    public interface IAreasService
    {
        Task<List<Area>> GetAreasAsync();
        Task<Area> PostAreaAsync(Area area);
    }
}
