namespace ReportesObra.Interfaces
{
    public interface IReportesService
    {
        Task<byte[]> GetReporteDetalles(int idBuilding, List<int> idApartments, List<int> idActivities, List<int> idElements, List<int>? idSubElements);
        //Task<byte[]> GetReporteDetalles(int idBuilding, int idApartment, List<int> actividades, int? idElement, int? idSubElement);
    }
}
