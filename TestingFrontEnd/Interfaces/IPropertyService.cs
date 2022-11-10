using SharedLibrary.Models;

namespace FrontEnd.Interfaces
{
    public interface IPropertyService
    {
        Task<List<Property>> GetPropertyAsync();
        Task<Property> PostPropertyAsync(Property property);
    }
}
