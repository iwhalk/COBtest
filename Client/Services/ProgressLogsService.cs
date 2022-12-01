using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;

namespace Obra.Client.Services
{
    public class ProgressLogsService : IProgressLogsService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public ProgressLogsService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<ProgressLog>> GetProgressLogsAsync()
        {
            if (_context.ProgressLog == null)
            {
                var response = await _repository.GetAsync<List<ProgressLog>>("api/ProgressLogs");

                if (response != null)
                {
                    _context.ProgressLog = response;
                    return _context.ProgressLog;
                }
            }

            return _context.ProgressLog;
        }

        public async Task<ProgressLog> PostProgressLogAsync(ProgressLog progressLog)
        {
            return null;
            //return await _repository.PostAsync("api/ProgressLogs", progressLog);
        }
    }
}
