using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using SharedLibrary;
using SharedLibrary.Models;

namespace ApiGateway.Services
{
    public class ProgressLogsService : GenericProxy, IProgressLogsService
    {
        public ProgressLogsService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<List<ProgressLog>>> GetProgressLogsAsync()
        {
            return await GetAsync<List<ProgressLog>>(path: "ProgressLogs");
        }

        public async Task<ApiResponse<ProgressLog>> PostProgressLogAsync(ProgressLog progressLog)
        {
            return await PostAsync<ProgressLog>(progressLog, path: "ProgressLogs");
        }
    }
}
