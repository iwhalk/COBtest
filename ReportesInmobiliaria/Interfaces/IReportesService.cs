using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IReportesService
    {
        Task<byte[]> GetReporteDetalles(int idBuilding, int idApartment);
        Task<byte[]> GetReporteAvance(int? idAparment);
        Task<ReporteAvance> GetReporteAvanceVista(int? idAparment);
    }
}
