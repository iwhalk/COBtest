using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IReportesService
    {
        Task<byte[]> GetReporteDetalles(int idBuilding, List<int> idApartments, List<int> idActivities, List<int> idElements, List<int>? idSubElements);
        Task<byte[]> GetReporteDetallesActividad(int idBuilding, List<int>? idActivities, List<int>? idElements, List<int>? idSubElements, List<int>? idApartments);
        Task<byte[]> GetReporteAvance(List<AparmentProgress> aparmentProgress);
        Task<List<AparmentProgress>> GetAparments(int? idAparment);
        Task<List<AparmentProgress>> GetActivitiesByAparment(int? idAparment);
        Task<List<ActivityProgressByAparment>> GetAparmentsByActivity(int? idActivity);
        Task<List<ActivityProgress>> GetActivityProgress(int? idBuilding, int? idActivity);
        Task<byte[]> GetReporteAvanceActividad(List<ActivityProgress> activityProgress);
        Task<byte[]> GetReporteAvancDeActividadPorDepartamento(List<AparmentProgress> activityProgress);
        Task<byte[]> GetReporteAvanceDeDepartamentoPorActividad(List<ActivityProgressByAparment> activityProgress);        
    }
}
