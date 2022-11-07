using Shared.Models;

namespace FrontEnd.Interfaces
{
    public interface IDescriptionService
    {
        Task<List<Description>> GetDescriptionAsync();
        Task<Description> PostDescriptionAsync(Description description);
    }
}
