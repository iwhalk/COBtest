using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IReportesService
    {
        Task<byte[]> GetReporteDetalles(int idBuilding, List<int> idApartments, List<int> idActivities, List<int> idElements, List<int>? idSubElements);
        Task<byte[]> GetReporteDetallesActividad(int idBuilding, List<int>? idActivities, List<int>? idElements, List<int>? idSubElements, List<int>? idApartments);
        Task<byte[]> GetReporteAvance(List<AparmentProgress> aparmentProgress);
        //Task <List<AparmentProgress>> GetAparmentsAsync(int? idAparment);
        //Task<byte[]> GetReporteAvance(int? idAparment);
        Task<List<AparmentProgress>> GetAparments(int? idAparment);
        Task<List<ActivityProgress>> GetActivityProgress(int idBuilding, int? idActivity);
    }
}
