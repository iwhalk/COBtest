using Shared.Models;
using Shared;

namespace ApiGateway.Interfaces
{
    public interface IServicesService
    {
        Task<ApiResponse<List<Service>>> GetServicesAsync();
        Task<ApiResponse<Service>> PostServicesAsync(Service service);
    }
}
