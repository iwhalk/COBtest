using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IReportesService
    {
        Task<List<DetalladoDepartamentos>> GetDataDetallesDepartamento(int idBuilding, List<int> idApartments, List<int>? idAreas, List<int>? idActivities, List<int>? idElements, List<int>? idSubElements);
        Task<byte[]> GetReporteDetallesDepartamento(List<DetalladoDepartamentos> detalladoDepartamentos, int? opcion);
        Task<List<DetalladoActividades>> GetDataDetallesActividad(int idBuilding, List<int>? idActivities, List<int>? idElements, List<int>? idSubElements, List<int>? idApartments);
        Task<byte[]> GetReporteDetallesActividad(List<DetalladoActividades> detalladoActividades, int? opcion);
        Task<byte[]> GetReporteAvance(List<AparmentProgress> aparmentProgress);
        Task<List<AparmentProgress>> GetAparments(int? idBuilding, int? idAparment);
        Task<List<AparmentProgress>> GetActivitiesByAparment(int? idBuilding, int? idAparment);
        Task<List<ActivityProgressByAparment>> GetAparmentsByActivity(int? idBuilding, int? idApartment);
        Task<List<ActivityProgress>> GetActivityProgress(int? idBuilding, int? idActivity);
        Task<byte[]> GetReporteAvanceActividad(List<ActivityProgress> activityProgress);
        Task<byte[]> GetReporteAvancDeActividadPorDepartamento(List<AparmentProgress> activityProgress);
        Task<byte[]> GetReporteAvanceDeDepartamentoPorActividad(List<ActivityProgressByAparment> activityProgress);        
    }
}
