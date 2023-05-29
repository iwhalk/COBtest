using Microsoft.Identity.Client;
using Obra.Client.Interfaces;
using Obra.Client.Models;
using Obra.Client.Stores;
using SharedLibrary;
using SharedLibrary.Models;

namespace Obra.Client.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public ReportsService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        //Nuevos EP's para las vistas de detalles por departamento y detalles por actividad
        //Detalles por departamento
        public async Task<List<DetalladoDepartamentos>> PostDataDetallesDepartamentos(ActivitiesDetail reporteDetalle)
        {
            return await _repository.PostAsync<List<DetalladoDepartamentos>>(reporteDetalle, path: "api/Reports/DataDetallesDepartamentos");
        }
        public async Task<byte[]> PostReporteDetallesPorDepartamento(List<DetalladoDepartamentos> detalladoDepartamentos, int? opcion)
        {
            if(opcion != null)
                return await _repository.PostAsync<byte[]>(detalladoDepartamentos, path: "api/Reports/ReporteDetallesDepartamentos?opcion=" + opcion.ToString());
            else
                return await _repository.PostAsync<byte[]>(detalladoDepartamentos, path: "api/Reports/ReporteDetallesDepartamentos");
        }
        //Destalles por actividad
        public async Task<List<DetalladoActividades>> PostDataDetallesActividades(ActivitiesDetail reporteDetalle)
        {
            return await _repository.PostAsync<List<DetalladoActividades>>(reporteDetalle, path: "api/Reports/DataDetallesActividades");
        }
        public async Task<byte[]> PostReporteDetallesPorActividadesAsync(List<DetalladoActividades> detalladoActividades, int? opcion)
        {
            if (opcion != null)
                return await _repository.PostAsync<byte[]>(detalladoActividades, path: "api/Reports/ReporteDetallesActividades?opcion=" + opcion.ToString());
            else
                return await _repository.PostAsync<byte[]>(detalladoActividades, path: "api/Reports/ReporteDetallesActividades");
        }

        public async Task<byte[]> PostReporteDetallesAsync(ActivitiesDetail reporteDetalle)
        {
            return await _repository.PostAsync<byte[]>(reporteDetalle, path: "api/Reports/Detalles");
        }
        public async Task<byte[]> PostReporteDetallesPorActividadAsync(ActivitiesDetail reporteDetalle)
        {
            return await _repository.PostAsync<byte[]>(reporteDetalle, path: "api/Reports/DetallesPorActividad");
        }

        //Evolucion del proyecto
        public async Task<byte[]> PostReporteEvolucionAsync(ActivitiesDetail reporteDetalle)
        {
            return await _repository.PostAsync<byte[]>(reporteDetalle, path: "api/Reports/ReporteEvolucion");
        }

        public async Task<byte[]> PostReporteTiempoAsync(ActivitiesDetail reporteDetalle)
        {
            return await _repository.PostAsync<byte[]>(reporteDetalle, path: "api/Reports/ReporteAvanceTiempo");
        }

        public async Task<byte[]> PostReporteCostosAsync(ActivitiesDetail reporteDetalle)
        {
            return await _repository.PostAsync<byte[]>(reporteDetalle, path: "api/Reports/ReporteAvanceCosto");
        }

        //MetodosReportes
        public async Task<List<AparmentProgress>?> GetProgressByAparmentDataViewAsync(int? idBuilding, int? idAparment)
        {
            Dictionary<string, string> parameters = new();
            parameters.Add("idBuilding", idBuilding.ToString());
            parameters.Add("idAparment", idAparment.ToString());
            return await _repository.GetAsync<List<AparmentProgress>?>(parameters, path: "api/Reports/ProgressByAparmentDataView");
        }
        public async Task<double> GetCostTotal(int? idBuilding, int? idAparment)
        {
            Dictionary<string, string> parameters = new();
            parameters.Add("idBuilding", idBuilding.ToString());
            parameters.Add("idAparment", idAparment.ToString());
            return await _repository.GetAsync<double>(parameters, path: "api/Reports/CostTotal");
        }

        public async Task<double> GetCostTotalActivity(int? idBuilding, int? idActivity)
        {
            Dictionary<string, string> parameters = new();
            parameters.Add("idBuilding", idBuilding.ToString());
            parameters.Add("idActivity", idActivity.ToString());
            return await _repository.GetAsync<double>(parameters, path: "api/Reports/CostTotalActivity");
        }

        public async Task<byte[]> PostProgressByAparmentPDFAsync(List<AparmentProgress> progressReportList, string subTitle)
        {
            return await _repository.PostAsync<byte[]>(progressReportList, path: "api/Reports/ProgressByAparmentPDF?subTitle=" + subTitle);
        }

        public async Task<List<ActivityProgress>?> GetProgressByActivityDataViewAsync(int? idBuilding, int? idActivity)
        {
            Dictionary<string, string> parameters = new();
            parameters.Add("idBuilding", idBuilding.ToString());
            parameters.Add("idActivity", idActivity.ToString());
            return await _repository.GetAsync<List<ActivityProgress>?>(parameters, path: "api/Reports/ProgressByActivityDataView");
        }

        public async Task<byte[]> PostProgressByActivityPDFAsync(List<ActivityProgress> progressReportList, string subTitle)
        {
            return await _repository.PostAsync<byte[]>(progressReportList, path: "api/Reports/ProgressByActivityPDF?subTitle=" + subTitle);
        }

        public async Task<List<AparmentProgress>?> GetProgressOfAparmentByActivityDataViewAsync(int? idBuilding, int? idActivity)
        {
            Dictionary<string, string> parameters = new();
            parameters.Add("idBuilding", idBuilding.ToString());
            parameters.Add("idActivity", idActivity.ToString());
            return await _repository.GetAsync<List<AparmentProgress>?>(parameters, path: "api/Reports/ProgressOfAparmentByActivityDataView");
        }

        public async Task<byte[]> PostProgressOfAparmentByActivityPDFAsync(List<AparmentProgress> progressReportList, bool all)
        {
            return await _repository.PostAsync<byte[]>(progressReportList, path: "api/Reports/ProgressOfAparmentByActivityPDF?all=" + all.ToString());
        }

        public async Task<List<ActivityProgressByAparment>?> GetProgressOfActivityByAparmentDataViewAsync(int? idBuilding, int? idActivity)
        {
            Dictionary<string, string> parameters = new();
            parameters.Add("idBuilding", idBuilding.ToString());
            parameters.Add("idApartment", idActivity.ToString());
            return await _repository.GetAsync<List<ActivityProgressByAparment>?>(parameters, path: "api/Reports/ProgressOfActivityByAparmentDataView");
        }

        public async Task<byte[]> PostProgressOfActivityByParmentPDFAsync(List<ActivityProgressByAparment> progressReportList, bool all)
        {
            return await _repository.PostAsync<byte[]>(progressReportList, path: "api/Reports/ProgressOfActivityByAparmentPDF?all="+ all.ToString());
        }
    }
}
