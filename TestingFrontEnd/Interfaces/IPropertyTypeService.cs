using Shared.Models;

namespace TestingFrontEnd.Interfaces
{
    public interface IPropertyTypeService
    {
        Task<List<PropertyType>> GetPropertyTypeAsync();
        Task<PropertyType> PostPropertyTypeAsync(PropertyType propertyType);
    }
}
