using Shared.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface IPropertyTypesService
    {
        Task<List<PropertyType>?> GetPropertyTypesAsync();
        Task<PropertyType?> GetPropertyTypeAsync(int id);
        Task<PropertyType?> GetPropertyTypeAsync(string name);
        Task<PropertyType?> CreatePropertyTypeAsync(PropertyType propertyType);
        Task<bool> UpdatePropertyTypeAsync(PropertyType propertyType);        
        Task<bool> DeletePropertyTypeAsync(string name);
    }
}
