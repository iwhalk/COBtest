using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IReportesService
    {
        Task<byte[]> GetReporteAvance(int? idAparment);
        Task <List<AparmentProgress>> GetAparments(int? idAparment);
        Task<byte[]> GetReporteDetalles(int ibBuilding, int idApartment);
    }
}
