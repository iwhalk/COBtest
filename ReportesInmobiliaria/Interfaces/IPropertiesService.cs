using Shared.Models;
using SharedLibrary.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface IPropertiesService
    {
        Task<List<Property>?> GetPropertiesAsync();
        Task<Property?> GetPropertyAsync(int id);
        //Task<Property?> GetPropertyAsync(string propertyName);
        Task<Property?> CreatePropertyAsync(Property property);
        Task<bool> UpdatePropertyAsync(Property property);
        Task<bool> DeletePropertyAsync(int id);
    }
}
