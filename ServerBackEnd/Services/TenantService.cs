using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using Shared;
using Shared.Models;

namespace ApiGateway.Services
{
    public class TenantService : GenericProxy, ITenantService
    {
        public TenantService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "ReportesInmobiliaria")
        {

        }

        public async Task<ApiResponse<List<Tenant>>> GetTenantAsync()
        {
            return await GetAsync<List<Tenant>>(path: "tenant");
        }

        public async Task<ApiResponse<Tenant>> PostTenantAsync(Tenant tenant)
        {
            return await PostAsync<Tenant>(tenant, path: "tenant");
        }
    }
}
