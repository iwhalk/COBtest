using SharedLibrary.Models;
using SharedLibrary;
using ApiGateway.Models;

namespace ApiGateway.Interfaces
{
    public interface IReportsService
    {
        Task<ApiResponse<List<DetalladoDepartamentos>>> PostDataDetallesPorDepartamentosAsync(ActivitiesDetail reporteDetalle);
        Task<ApiResponse<byte[]>> PostReporteDetallesPorDepartamentosAsync(List<DetalladoDepartamentos> detalladoDepartamentos, int? opcion);
        Task<ApiResponse<List<DetalladoActividades>>> PostDataDetallesPorActividadesAsync(ActivitiesDetail reporteDetalle);        
        Task<ApiResponse<byte[]>> PostReporteDetallesPorActividadesAsync(List<DetalladoActividades> detalladoActividades, int? opcion);

        Task<ApiResponse<List<AparmentProgress>>?> GetProgressByAparmentViewAsync(int? id);
        Task<ApiResponse<byte[]>> PostProgressByAparmentPDFAsync(List<AparmentProgress> progressReport);

        Task<ApiResponse<List<ActivityProgress>>?> GetProgressByActivityViewAsync(int? idBuilding, int? idActivity);
        Task<ApiResponse<byte[]>> PostProgressByActivityPDFAsync(List<ActivityProgress> progressReport);

        Task<ApiResponse<List<AparmentProgress>>?> GetProgressOfAparmentByActivityViewAsync(int? idActivity);
        Task<ApiResponse<byte[]>> PostProgressOfAparmentByActivityPDFAsync(List<AparmentProgress> progressReport);

        Task<ApiResponse<List<ActivityProgressByAparment>>?> GetProgressOfActivityByAparmentViewAsync(int? idActivity);
        Task<ApiResponse<byte[]>> PostProgressOfActivityByAparmentPDFAsync(List<ActivityProgressByAparment> progressReport);

    }
}
