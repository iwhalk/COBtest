using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace Obra.Client.Services
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

        public async Task<ProgressReport> GetProgressReportAsync(int id)
        {
            return await _repository.GetAsync<ProgressReport>(id, path: "api/ProgressReport");
        }

        public async Task<List<ProgressReport>> GetProgressReportsAsync(int? idProgressReport = null, int? idBuilding = null, int? idApartment = null, int? idArea = null, int? idElement = null, int? idSubElement = null, string? idSupervisor = null, bool includeProgressLogs = false)
        {
            Dictionary<string, string> parameters = new();

            if (idProgressReport != null && idProgressReport > 0)
            {
                parameters.Add("idProgressReport", idProgressReport.ToString());
            }
            if (idBuilding != null && idBuilding > 0)
            {
                parameters.Add("idBuilding", idBuilding.ToString());
            }
            if (idApartment != null && idApartment > 0)
            {
                parameters.Add("idApartment", idApartment.ToString());
            }
            if (idArea != null && idArea > 0)
            {
                parameters.Add("idArea", idArea.ToString());
            }
            if (idElement != null && idElement > 0)
            {
                parameters.Add("idElement", idElement.ToString());
            }
            if (idSubElement != null && idSubElement > 0)
            {
                parameters.Add("idSubElement", idSubElement.ToString());
            }
            if (idSupervisor is not null)
            {
                parameters.Add("idSupervisor", idSupervisor);
            }

            parameters.Add("includeProgressLogs", includeProgressLogs.ToString());

            return await _repository.GetAsync<List<ProgressReport>>(parameters, path: "api/ProgressReport"); ;
        }

        public async Task<ObjectAccessUser> GetObjectAccessAsync(string idSupervisor)
        {
            //Dictionary<string, string> parameters = new();
            //parameters.Add("idSupervisor", idSupervisor);
            return await _repository.GetAsync<ObjectAccessUser>(idSupervisor, path: "api/ProgressReport/ObjectAccess");
        }

        public async Task<ProgressReport> PostProgressReportAsync(ProgressReport progressReport)
        {
            return await _repository.PostAsync(progressReport, path: "api/ProgressReport");
        }
    }
}
