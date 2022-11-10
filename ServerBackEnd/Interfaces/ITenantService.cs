using Shared.Models;
using Shared;
using SharedLibrary.Models;

namespace ApiGateway.Interfaces
{
    public interface ITenantService
    {
        Task<ApiResponse<List<Tenant>>> GetTenantAsync();
        Task<ApiResponse<Tenant>> PostTenantAsync(Tenant tenant);
    }
}
