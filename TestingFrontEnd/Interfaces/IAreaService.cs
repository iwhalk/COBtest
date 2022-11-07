using Shared.Models;

namespace TestingFrontEnd.Interfaces
{
    public interface IAreaService
    {
        Task<List<Area>> GetAreaAsync();
        Task<Area> PostAreaAsync(Area area);
    }
}
