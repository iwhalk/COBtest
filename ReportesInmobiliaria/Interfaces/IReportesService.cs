namespace ReportesObra.Interfaces
{
    public interface IReportesService
    {
        Task<byte[]> GetReporteDetalles(int ibBuilding, int idApartment);
        Task<byte[]> GetReporteDetalles(int ibBuilding, int idApartment, List<int> actividades);
    }
}
