using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using SharedLibrary;
using SharedLibrary.Models;

namespace ApiGateway.Services
{
    public class BuildingsService : GenericProxy, IBuildingsService
    {
        public BuildingsService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<List<Building>>> GetBuildingsAsync()
        {
            return await GetAsync<List<Building>>(path: "Buildings");
        }

        public async Task<ApiResponse<Building>> PostBuildingAsync(Building building)
        {
            return await PostAsync<Building>(building, path: "Buildings");
        }
    }
}
