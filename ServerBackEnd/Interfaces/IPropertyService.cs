using Shared.Models;
using Shared;
using SharedLibrary.Models;

namespace ApiGateway.Interfaces
{
    public interface IPropertyService
    {
        Task<ApiResponse<List<Property>>> GetPropertyAsync();
        Task<ApiResponse<Property>> PostPropertyAsync(Property property);
        Task<ApiResponse<Property>> PutPropertyAsync(Property property);
    }
}
