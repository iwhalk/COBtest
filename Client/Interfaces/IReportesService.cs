using SharedLibrary.Models;
using SharedLibrary;
using Obra.Client.Models;

namespace Obra.Client.Interfaces
{
    public interface IReportesService
    {
        Task<byte[]> PostReporteDetallesAsync(ReporteDetalle reporteDetalle);
    }
}
