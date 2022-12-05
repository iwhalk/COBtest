using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using SharedLibrary;
using SharedLibrary.Models;
using System.Data;

namespace ApiGateway.Services
{
    public class BuildingsService : GenericProxy, IBuildingsService
    {
        public BuildingsService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<Building>> GetBuildingAsync(int id)
        {
            return await GetAsync<Building>(id, path: "Building");
        }
        public async Task<ApiResponse<List<Building>>> GetBuildingsAsync()
        {
            return await GetAsync<List<Building>>(path: "Buildings");
        }
        public async Task<ApiResponse<Building>> PostBuildingAsync(Building building)
        {
            return await PostAsync<Building>(building, path: "Building");
        }
    }
}
