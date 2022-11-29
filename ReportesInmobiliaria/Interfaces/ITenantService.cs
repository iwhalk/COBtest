using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface ITenantService
    {
        Task<List<Tenant?>> GetTenantAsync();
        Task<Tenant?> CreateTenantAsync(Tenant tenant);
    }
}
