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
        public async Task<List<AparmentProgress>?> GetProgressByAparmentDataViewAsync(int? id)
        {
            Dictionary<string, string> parameters = new();
            parameters.Add("id", id.ToString());
            return await _repository.GetAsync<List<AparmentProgress>?>(parameters, path: "api/Reports/ProgressByAparmentDataView");
        }
        public async Task<byte[]> PostProgressByAparmentPDFAsync(List<AparmentProgress> progressReportList)
        {
            return await _repository.PostAsync<byte[]>(progressReportList, path: "api/Reports/ProgressByAparmentPDF");
        }
    }
}
