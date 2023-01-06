using Obra.Client.Interfaces;
using Obra.Client.Models;
using Obra.Client.Stores;
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

        public async Task<byte[]> PostReporteDetallesAsync(ActivitiesDetail reporteDetalle)
        {
            return await _repository.PostAsync<byte[]>(reporteDetalle, path: "api/Reports/Detalles");
        }
        public async Task<byte[]> PostReporteDetallesPorActividadAsync(ActivitiesDetail reporteDetalle)
        {
            return await _repository.PostAsync<byte[]>(reporteDetalle, path: "api/Reports/DetallesPorActividad");
        }

        //MetodosReportes
        public async Task<List<AparmentProgress>?> GetProgressByAparmentDataViewAsync(int? idAparment)
        {
            Dictionary<string, string> parameters = new();
            parameters.Add("idAparment", idAparment.ToString());
            return await _repository.GetAsync<List<AparmentProgress>?>(parameters, path: "api/Reports/ProgressByAparmentDataView");
        }
        public async Task<byte[]> PostProgressByAparmentPDFAsync(List<AparmentProgress> progressReportList)
        {
            return await _repository.PostAsync<byte[]>(progressReportList, path: "api/Reports/ProgressByAparmentPDF");
        }

        public async Task<List<ActivityProgress>?> GetProgressByActivityDataViewAsync(int? idBuilding, int? idActivity)
        {
            Dictionary<string, string> parameters = new();
            parameters.Add("idBuilding", idBuilding.ToString());
            parameters.Add("idActivity", idActivity.ToString());
            return await _repository.GetAsync<List<ActivityProgress>?>(parameters, path: "api/Reports/ProgressByActivityDataView");
        }

        public async Task<byte[]> PostProgressByActivityPDFAsync(List<ActivityProgress> progressReportList)
        {
            return await _repository.PostAsync<byte[]>(progressReportList, path: "api/Reports/ProgressByActivityPDF");
        }

        public async Task<List<AparmentProgress>?> GetProgressOfAparmentByActivityDataViewAsync(int? idAparment)
        {
            Dictionary<string, string> parameters = new();
            parameters.Add("idAparment", idAparment.ToString());
            return await _repository.GetAsync<List<AparmentProgress>?>(parameters, path: "api/Reports/ProgressOfAparmentByActivityDataView");
        }

        public async Task<byte[]> PostProgressOfActivitybyActivityPDFAsync(List<AparmentProgress> progressReportList)
        {
            return await _repository.PostAsync<byte[]>(progressReportList, path: "api/Reports/ProgressOfActivityByAparmentPDF");
        }
    }
}
