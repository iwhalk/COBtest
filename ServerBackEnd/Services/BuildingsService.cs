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
            Dictionary<string, string> parameters = new();

            if (id != null && id > 0)
            {
                parameters.Add("id", id.ToString());
            }

            return await GetAsync<Building>(path: "Building", parameters: parameters);
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
