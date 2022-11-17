using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Services
{
    public class AreasService : GenericProxy, IAreaService
    {
        public AreasService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }
        public async Task<ApiResponse<List<Area>>> GetAreaAsync()
        {
            return await GetAsync<List<Area>>(path: "Areas");
        }
        public async Task<ApiResponse<List<AreaService>>> GetAreaServicesAsync()
        {
            return await GetAsync<List<AreaService>>(path: "AreaServices");
        }

        public async Task<ApiResponse<Area>> PostAreaAsync(Area area)
        {
            return await PostAsync<Area>(area, path: "Area");
        }
    }
}
