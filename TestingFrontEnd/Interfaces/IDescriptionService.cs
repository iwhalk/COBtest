using Shared.Models;

namespace TestingFrontEnd.Interfaces
{
    public interface IDescriptionService
    {
        Task<List<Description>> GetDescriptionAsync();
        Task<Description> PostDescriptionAsync(Description description);
    }
}
