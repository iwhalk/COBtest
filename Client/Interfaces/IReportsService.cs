using SharedLibrary.Models;
using SharedLibrary;
using Obra.Client.Models;

namespace Obra.Client.Interfaces
{
    public interface IReportsService
    {
        Task<byte[]> PostReporteDetallesAsync(ActivitiesDetail reporteDetalle);
        Task<byte[]> PostReporteDetallesPorActividadAsync(ActivitiesDetail reporteDetalle);
        //MetodosReportes
        Task<List<AparmentProgress>?> GetProgressByAparmentDataViewAsync(int? id);
        Task<byte[]> PostProgressByAparmentPDFAsync(List<AparmentProgress> progressReportList);
    }
}
