using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IReportesService
    {
        Task<byte[]> GetReporteDetalles(int idApartment);
        Task<byte[]> GetReporteAvance(int? idAparment);
        Task<ReporteAvance> GetReporteAvanceVista(int? idAparment);
    }
}
