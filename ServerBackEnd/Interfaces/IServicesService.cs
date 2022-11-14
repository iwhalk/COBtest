using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IServicesService
    {
        Task<ApiResponse<List<Service>>> GetServicesAsync();
        Task<ApiResponse<Service>> PostServicesAsync(Service service);
    }
}
