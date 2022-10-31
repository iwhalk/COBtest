using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using Shared;
using Shared.Models;

namespace ApiGateway.Services
{
    public class ServicesService : GenericProxy, IServicesService
    {
        public ServicesService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "ReportesInmobiliaria")
        {

        }

        public async Task<ApiResponse<List<Service>>> GetServicesAsync()
        {
            return await GetAsync<List<Service>>(path: "service");
        }

        public async Task<ApiResponse<Service>> PostServicesAsync(Service service)
        {
            return await PostAsync<Service>(service, path: "service");
        }
    }
}
