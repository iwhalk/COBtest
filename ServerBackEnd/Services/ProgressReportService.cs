using SharedLibrary.Models;
using SharedLibrary;
using ApiGateway.Proxies;
using ApiGateway.Interfaces;
using System.Xml.Linq;

namespace ApiGateway.Services
{
    public class ProgressReportService : GenericProxy, IProgressReportService
    {
        public ProgressReportService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<List<ProgressReport>>> GetProgressReportsAsync()
        {
            return await GetAsync<List<ProgressReport>>(path: "ProgressReport");
        }

        public async Task<ApiResponse<ProgressReport>> PostProgressReportAsync(ProgressReport progressReport)
        {
            return await PostAsync<ProgressReport>(progressReport, path: "ProgressReport");
        }
    }
}
