using Obra.Client.Interfaces;
using Obra.Client.Models;
using Obra.Client.Stores;

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

        public async Task<byte[]> PostReporteDetallesAsync(ReporteDetalle reporteDetalle)
        {
            return await _repository.PostAsync<byte[]>(reporteDetalle, path: "api/ReporteDetalle");
        }
    }
}
