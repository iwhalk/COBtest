using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IDescriptionService
    {
        Task<List<Description?>> GetDescriptionAsync();
        Task<Description?> CreateDescriptionAsync(Description description);
    }
}
