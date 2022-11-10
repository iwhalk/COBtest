using Shared.Models;

namespace FrontEnd.Interfaces
{
    public interface ITenantService
    {
        Task<List<Tenant>> GetTenantAsync();
        Task<Tenant> PostTenantAsync(Tenant tenant);
    }
}
