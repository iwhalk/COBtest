using SharedLibrary.Models;
using SharedLibrary;
using ApiGateway.Models;

namespace ApiGateway.Interfaces
{
    public interface IReportsService
    {
        Task<ApiResponse<byte[]>> PostReporteDetallesAsync(ActivitiesDetail reporteDetalle);
        Task<ApiResponse<byte[]>> PostReporteDetallesPorActividadesAsync(ActivitiesDetail reporteDetalle);

        Task<ApiResponse<List<AparmentProgress>>?> GetProgressByAparmentViewAsync(int? id);
        Task<ApiResponse<byte[]>> PostProgressByAparmentPDFAsync(List<AparmentProgress> progressReport);

        Task<ApiResponse<List<ActivityProgress>>?> GetProgressByActivityViewAsync(int? idBuilding, int? idActivity);
        Task<ApiResponse<byte[]>> PostProgressByActivityPDFAsync(List<ActivityProgress> progressReport);

        Task<ApiResponse<List<AparmentProgress>>?> GetProgressOfAparmentByActivityViewAsync(int? idActivity);
        Task<ApiResponse<byte[]>> PostProgressOfAparmentByActivityPDFAsync(List<AparmentProgress> progressReport);

    }
}
