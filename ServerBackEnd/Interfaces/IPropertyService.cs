using Shared.Models;
using Shared;

namespace ApiGateway.Interfaces
{
    public interface IPropertyService
    {
        Task<ApiResponse<List<Property>>> GetPropertyAsync();
        Task<ApiResponse<Property>> PostPropertyAsync(Property property);
    }
}
