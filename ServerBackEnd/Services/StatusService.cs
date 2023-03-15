using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Services
{
    public class StatusService : GenericProxy, IStatusService
    {
        public StatusService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<List<Status>>> GetStatusesAsync()
        {
            return await GetAsync<List<Status>>(path: "Statuses");
        }
        public async Task<ApiResponse<Status>> GetStatusAsync(int idStatus)
        {
            return await GetAsync<Status>(idStatus, path: "Status");
        }
    }
}
