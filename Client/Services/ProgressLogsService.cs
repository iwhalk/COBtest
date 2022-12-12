using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;
using System.Net.NetworkInformation;

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

        public async Task<ProgressLog> GetProgressLogAsync(int id)
        {
            return await _repository.GetAsync<ProgressLog>(id, path: "api/ProgressLogs");
        }

        public async Task<List<ProgressLog>> GetProgressLogsAsync(int? idProgressLog = null, int? idProgressReport = null, int? idStatus = null, string? idSupervisor = null)
        {
            Dictionary<string, string> parameters = new();

            if (idProgressLog != null && idProgressLog > 0)
            {
                parameters.Add("idProgressLog", idProgressLog.ToString());
            }
            if (idProgressReport != null && idProgressReport > 0)
            {
                parameters.Add("idProgressReport", idProgressReport.ToString());
            }
            if (idStatus != null && idStatus > 0)
            {
                parameters.Add("idStatus", idStatus.ToString());
            }
            if (idSupervisor is not null)
            {
                parameters.Add("idSupervisor", idSupervisor);
            }

            //if (_context.ProgressLog == null)
            //{
            //    var response = await _repository.GetAsync<List<ProgressLog>>(path: "api/ProgressLogs");

            //    if (response != null)
            //    {
            //        _context.ProgressLog = response;
            //        return _context.ProgressLog;
            //    }
            //}

            return await _repository.GetAsync<List<ProgressLog>>(parameters, path: "api/ProgressLogs");
        }
        public async Task<ProgressLog> PostProgressLogAsync(ProgressLog progressLog)
        {
            return await _repository.PostAsync(progressLog, path: "api/ProgressLogs");
        }
    }
}
