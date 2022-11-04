using Shared.Models;
using Shared;

namespace ApiGateway.Interfaces
{
    public interface ITenantService
    {
        Task<ApiResponse<List<Tenant>>> GetTenantAsync();
        Task<ApiResponse<Tenant>> PostTenantAsync(Tenant tenant);
    }
}
