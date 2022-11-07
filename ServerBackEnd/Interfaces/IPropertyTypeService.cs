using Shared;
using Shared.Models;

namespace ApiGateway.Interfaces
{
    public interface IPropertyTypeService
    {
        Task<ApiResponse<List<PropertyType>>> GetPropertyTypeAsync();
        Task<ApiResponse<PropertyType>> PostPropertyTypeAsync(PropertyType propertyType);
    }
}
