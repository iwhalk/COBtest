namespace ReportesObra.Interfaces
{
    public interface IReportesService
    {
        Task<byte[]> GetReporteDetalles(int idApartment);
        Task<byte[]> GetReporteAvance(int? idAparment);
    }
}
