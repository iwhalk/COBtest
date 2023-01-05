using SharedLibrary.Models;
using SharedLibrary;
using Obra.Client.Models;

namespace Obra.Client.Interfaces
{
    public interface IReportesService
    {
        Task<byte[]> PostReporteDetallesAsync(ActivitiesDetail reporteDetalle);
        Task<byte[]> PostReporteDetallesPorActividadAsync(ActivitiesDetail reporteDetalle);
    }
}
