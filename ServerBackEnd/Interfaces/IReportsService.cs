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
        Task<ApiResponse<byte[]>> PostReporteEvolucionAsync(ActivitiesDetail reporteDetalle);
        Task<ApiResponse<byte[]>> PostReporteAvanceCostoAsync(ActivitiesDetail reporteDetalle);
        Task<ApiResponse<byte[]>> PostReporteAvanceTiempoAsync(ActivitiesDetail reporteDetalle);

        Task<ApiResponse<List<AparmentProgress>>?> GetProgressByAparmentViewAsync(int? idBuilding, int? id);
        Task<ApiResponse<double>> GetCostTotal(int? idBuilding, int? id);
        Task<ApiResponse<double>> GetCostTotalActivity(int? idBuilding, int? id);

        Task<ApiResponse<double>> GetCostTotalActivitiesByAparment(int? idBuilding, int? id);
        Task<ApiResponse<double>> GetCostAparmentsByActivity(int? idBuilding, int? id);

        Task<ApiResponse<byte[]>> PostProgressByAparmentPDFAsync(List<AparmentProgress> progressReport, string subTitle);

        Task<ApiResponse<List<ActivityProgress>>?> GetProgressByActivityViewAsync(int? idBuilding, int? idActivity);
        Task<ApiResponse<byte[]>> PostProgressByActivityPDFAsync(List<ActivityProgress> progressReport, string subTitle);

        Task<ApiResponse<List<AparmentProgress>>?> GetProgressOfAparmentByActivityViewAsync(int? idBuilding, int? idActivity);
        Task<ApiResponse<byte[]>> PostProgressOfAparmentByActivityPDFAsync(List<AparmentProgress> progressReport, bool all);

        Task<ApiResponse<List<ActivityProgressByAparment>>?> GetProgressOfActivityByAparmentViewAsync(int? idBuilding, int? idActivity);
        Task<ApiResponse<byte[]>> PostProgressOfActivityByAparmentPDFAsync(List<ActivityProgressByAparment> progressReport, bool all);

    }
}
