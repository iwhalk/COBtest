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

        public async Task<ApiResponse<ProgressLog>> GetProgressLogAsync(int id)
        {
            return await GetAsync<ProgressLog>(id, path: "ProgressLog");
        }

        public async Task<ApiResponse<List<ProgressLog>>> GetProgressLogsAsync(int? idProgressLog, int? idProgressReport, int? idStatus, string? idSupervisor)
        {
            Dictionary<string, string> parameters = new();

            if (idProgressLog != null && idProgressLog > 0)
            {
                parameters.Add("idProgressLog", idProgressLog.ToString());
            }
            if (idProgressReport != null && idProgressReport > 0)
            {
                parameters.Add("idProgressReport", idProgressReport.ToString());
            }
            if (idStatus != null && idStatus > 0)
            {
                parameters.Add("idStatus", idStatus.ToString());
            }
            if (idSupervisor is not null)
            {
                parameters.Add("idSupervisor", idSupervisor);
            }

            return await GetAsync<List<ProgressLog>>(path: "ProgressLogs", parameters: parameters);
        }

        public async Task<ApiResponse<ProgressLog>> PostProgressLogAsync(ProgressLog progressLog)
        {
            return await PostAsync<ProgressLog>(progressLog, path: "ProgressLog");
        }
    }
}
