using Shared.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface ITenantService
    {
        Task<List<Tenant>> GetTenant();
    }
}
