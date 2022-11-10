using Shared.Models;
using SharedLibrary.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface ITenantService
    {
        Task<List<Tenant?>> GetTenantAsync();
        Task<Tenant?> CreateTenantAsync(Tenant tenant);
    }
}
