using SharedLibrary.Models;
using SharedLibrary;
using ApiGateway.Models;

namespace ApiGateway.Interfaces
{
    public interface IReportesService
    {
        Task<ApiResponse<byte[]>> PostReporteDetallesAsync(ActivitiesDetail reporteDetalle);
        Task<ApiResponse<byte[]>> PostReporteDetallesPorActividadesAsync(ActivitiesDetail reporteDetalle);
    }
}
