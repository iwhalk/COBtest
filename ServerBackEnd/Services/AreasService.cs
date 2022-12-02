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
            Dictionary<string, string> parameters = new();

            if (id != null && id > 0)
            {
                parameters.Add("id", id.ToString());
            }

            return await GetAsync<Area>(id, path: "Area", parameters: parameters);
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
