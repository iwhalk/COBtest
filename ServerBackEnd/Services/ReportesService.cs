using ApiGateway.Interfaces;
using ApiGateway.Models;
using ApiGateway.Proxies;
using SharedLibrary;
using SharedLibrary.Models;

namespace ApiGateway.Services
{
    public class ReportesService : GenericProxy, IReportesService
    {
        public ReportesService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<byte[]>> PostReporteDetallesAsync(ActivitiesDetail reporteDetalle)
        {
            return await PostAsync<byte[]>(reporteDetalle, path: "ReporteDetalles");
        }

        public async Task<ApiResponse<byte[]>> PostReporteDetallesPorActividadesAsync(ActivitiesDetail reporteDetalle)
        {
            return await PostAsync<byte[]>(reporteDetalle, path: "ReporteDetalladoPorActividad");
        }
    }
}
