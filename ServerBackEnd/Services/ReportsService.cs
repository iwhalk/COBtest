using ApiGateway.Interfaces;
using ApiGateway.Models;
using ApiGateway.Proxies;
using SharedLibrary;
using SharedLibrary.Models;

namespace ApiGateway.Services
{
    public class ReportsService : GenericProxy, IReportsService
    {
        public ReportsService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
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
        //Reports
        public async Task<ApiResponse<List<AparmentProgress>>?> GetProgressByAparmentViewAsync(int? id)
        {
            Dictionary<string, string> parameters = new();
            if (id != null)
            {
                parameters.Add("idAparment", id.ToString());
            }
            return await GetAsync<List<AparmentProgress>?>(path: "ReportProgressByAparmentView", parameters: parameters);
        }
        public async Task<ApiResponse<byte[]>> PostProgressByAparmentPDFAsync(List<AparmentProgress> progressReport)
        {
            return await PostAsync<byte[]>(progressReport, path: "ReportProgressByAparmentPDF");
        }
    }
}
