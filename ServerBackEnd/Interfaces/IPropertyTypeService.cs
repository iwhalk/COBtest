using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IPropertyTypeService
    {
        Task<ApiResponse<List<PropertyType>>> GetPropertyTypeAsync();
        Task<ApiResponse<PropertyType>> PostPropertyTypeAsync(PropertyType propertyType);
    }
}
