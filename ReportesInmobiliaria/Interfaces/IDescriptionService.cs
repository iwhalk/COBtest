using SharedLibrary.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface IDescriptionService
    {
        Task<List<Description?>> GetDescriptionAsync();
        Task<Description?> CreateDescriptionAsync(Description description);
    }
}
