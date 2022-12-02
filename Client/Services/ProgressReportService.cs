using Client.Interfaces;
using Client.Stores;
using SharedLibrary.Models;
using System.Reflection.Metadata;

namespace Client.Services
{
    public class ProgressReportService : IProgressReportService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public ProgressReportService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<ProgressReport>> GetProgressReportsAsync()
        {
            if (_context.ProgressReport == null)
            {
                var response = await _repository.GetAsync<List<ProgressReport>>("api/ProgressReport");

                if (response != null)
                {
                    _context.ProgressReport = response;
                    return _context.ProgressReport;
                }
            }

            return _context.ProgressReport;
        }

        public async Task<ProgressReport> PostProgressReportAsync(ProgressReport progressReport)
        {
            return await _repository.PostAsync("api/ProgressReport", progressReport);
        }
    }
}
