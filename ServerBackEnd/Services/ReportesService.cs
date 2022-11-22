using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using SharedLibrary;

namespace ApiGateway.Services
{
    public class ReportesService : GenericProxy, IReportesService
    {
        public ReportesService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<byte[]>> GEetReporteReceptionCertificateAsync(int IdReceptionCertificate)
        {
            return await GetAsync<byte[]>(path: $"/ReporteActaEntrega/?IdReceptionCertificate={IdReceptionCertificate}");
        }

        public async Task<ApiResponse<byte[]>> GetReporteLessorsAsync(int Lessor)
        {
            return await GetAsync<byte[]>(path: "/ReporteArrendores");
        }
    }
}
