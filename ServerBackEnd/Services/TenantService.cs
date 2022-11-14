using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using Shared;
using Shared.Models;
using SharedLibrary.Models;

namespace ApiGateway.Services
{
    public class TenantService : GenericProxy, ITenantService
    {
        public TenantService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<List<Tenant>>> GetTenantAsync()
        {
            return await GetAsync<List<Tenant>>(path: "Tenant");
        }

        public async Task<ApiResponse<Tenant>> PostTenantAsync(Tenant tenant)
        {
            return await PostAsync<Tenant>(tenant, path: "Tenant");
        }
    }
}
