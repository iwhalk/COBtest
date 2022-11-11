using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using Shared;

namespace ApiGateway.Services
{
    public class ReportesService : GenericProxy, IReportesService
    {
        public ReportesService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<byte[]>> GetReporteFeaturesAsync(int IdFeature)
        {
            return await GetAsync<byte[]>(path: "/ReporteFeatures");
        }

        public async Task<ApiResponse<byte[]>> GetReporteLessorsAsync(int Lessor)
        {
            return await GetAsync<byte[]>(path: "/ReporteArrendores");
        }
    }
}
