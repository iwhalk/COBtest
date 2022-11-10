using Shared.Models;
using SharedLibrary.Models;

namespace TestingFrontEnd.Interfaces
{
    public interface IPropertyService
    {
        Task<List<Property>> GetPropertyAsync();
        Task<Property> PostPropertyAsync(Property property);
    }
}
