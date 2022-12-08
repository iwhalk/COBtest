using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using SharedLibrary;
using SharedLibrary.Models;

namespace ApiGateway.Services
{
    public class AreasService : GenericProxy, IAreasService
    {
        public AreasService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<Area>> GetAreaAsync(int id)
        {
            return await GetAsync<Area>(id, path: "Area");
        }

        public async Task<ApiResponse<List<Area>>> GetAreasAsync()
        {
            return await GetAsync<List<Area>>(path: "Areas");
        }

        public async Task<ApiResponse<Area>> PostAreaAsync(Area area)
        {
            return await PostAsync<Area>(area, path: "Area");
        }
    }
}
