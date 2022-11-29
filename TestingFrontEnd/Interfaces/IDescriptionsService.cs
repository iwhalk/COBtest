using SharedLibrary.Models;

namespace FrontEnd.Interfaces
{
    public interface IDescriptionsService
    {
        Task<List<Description>> GetDescriptionAsync();
        Task<Description> PostDescriptionAsync(Description description);
    }
}
