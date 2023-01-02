using Obra.Client.Interfaces;
using Obra.Client.Models;
using Obra.Client.Stores;
using SharedLibrary.Models;

namespace Obra.Client.Services
{
    public class ReportesService : IReportesService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public ReportesService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<byte[]> PostReporteDetallesAsync(ActivitiesDetail reporteDetalle)
        {
            return await _repository.PostAsync<byte[]>(reporteDetalle, path: "api/Reportes/Detalles");
        }
        public async Task<byte[]> PostReporteDetallesPorActividadAsync(ActivitiesDetail reporteDetalle)
        {
            return await _repository.PostAsync<byte[]>(reporteDetalle, path: "api/Reportes/DetallesPorActividad");
        }
    }
}
