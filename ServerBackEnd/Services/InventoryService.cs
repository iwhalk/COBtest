using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using Shared;
using Shared.Models;
using System.Security.Claims;

namespace ApiGateway.Services
{
    public class InventoryService : GenericProxy, IInventoryService
    {
        public InventoryService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "ReportesInmobiliaria")
        {

        }

        public async Task<ApiResponse<List<Inventory>>> GetInventoryAsync()
        {
            return await GetAsync<List<Inventory>>(path: "inventory");
        }

        public async Task<ApiResponse<Inventory>> PostInventoryAsync(Inventory inventory)
        {
            return await PostAsync<Inventory>(inventory, path: "inventory");
        }
    }
}
