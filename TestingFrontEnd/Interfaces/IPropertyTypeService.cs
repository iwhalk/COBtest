using Shared.Models;

namespace FrontEnd.Interfaces
{
    public interface IPropertyTypeService
    {
        Task<List<PropertyType>> GetPropertyTypeAsync();
        Task<PropertyType> PostPropertyTypeAsync(PropertyType propertyType);
    }
}
