using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IReportesService
    {
        Task<List<DetalladoDepartamentos>> GetDataDetallesDepartamento(int idBuilding, List<int> idApartments, List<int>? idAreas, List<int>? idActivities, List<int>? idElements, List<int>? idSubElements, int? statusOption);
        Task<byte[]> GetReporteDetallesDepartamento(List<DetalladoDepartamentos> detalladoDepartamentos);
        Task<List<DetalladoActividades>> GetDataDetallesActividad(int idBuilding, List<int>? idActivities, List<int>? idElements, List<int>? idSubElements, List<int>? idApartments, int? statusOption);
        Task<byte[]> GetReporteDetallesActividad(List<DetalladoActividades> detalladoActividades);

        Task<byte[]> GetReportEvolution(int idBuilding, DateTime inicio, DateTime fin, List<int>? idApartments, List<int>? idAreas, List<int>? idActivities, List<int>? idElements, List<int>? idSubElements, int? status, bool? withActivities);

        Task<byte[]> GetReportAdvanceCost(int idBuilding, List<int>? idApartments, List<int>? idAreas,
            List<int>? idActivities, List<int>? idElements, List<int>? idSubElements, int? status);

        Task<byte[]> GetReportAdvanceTime(int idBuilding, List<int>? idApartments, List<int>? idAreas,
            List<int>? idActivities, List<int>? idElements, List<int>? idSubElements, int? status);

        Task<byte[]> GetReporteAvance(List<AparmentProgress> aparmentProgress, string subTitle);
        Task<List<AparmentProgress>> GetAparments(int? idBuilding, int? idAparment);
        Task<double> GetAparmentTotalCost(int? idBuilding, int? idAparment);
        Task<double> GetActivityTotalCost(int? idBuilding, int? IdActivity);

        Task<List<AparmentProgress>> GetActivitiesByAparment(int? idBuilding, int? idAparment);
        Task<List<ActivityProgressByAparment>> GetAparmentsByActivity(int? idBuilding, int? idApartment);
        Task<List<ActivityProgress>> GetActivityProgress(int? idBuilding, int? idActivity);
        Task<byte[]> GetReporteAvanceActividad(List<ActivityProgress> activityProgress, string subTitle);
        Task<byte[]> GetReporteAvancDeActividadPorDepartamento(List<AparmentProgress> activityProgress, bool all);
        Task<byte[]> GetReporteAvanceDeDepartamentoPorActividad(List<ActivityProgressByAparment> activityProgress, bool all);        
    }
}
