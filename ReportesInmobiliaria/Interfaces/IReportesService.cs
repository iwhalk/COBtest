namespace ReportesObra.Interfaces
{
    public interface IReportesService
    {
        Task<byte[]> GetReporteDetalles(int idApartment);
    }
}
