using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface ITenantService
    {
        Task<ApiResponse<List<Tenant>>> GetTenantAsync();
        Task<ApiResponse<Tenant>> PostTenantAsync(Tenant tenant);
    }
}
