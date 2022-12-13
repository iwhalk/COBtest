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

        public async Task<List<ProgressReport>> GetProgressReportsAsync(int? idProgressReport = null, int? idBuilding = null, int? idAparment = null, int? idArea = null, int? idElemnet = null, int? idSubElement = null, string? idSupervisor = null)
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
            if (idAparment != null && idAparment > 0)
            {
                parameters.Add("idAparment", idAparment.ToString());
            }
            if (idArea != null && idArea > 0)
            {
                parameters.Add("idArea", idArea.ToString());
            }
            if (idElemnet != null && idElemnet > 0)
            {
                parameters.Add("idElemnet", idElemnet.ToString());
            }
            if (idSubElement != null && idSubElement > 0)
            {
                parameters.Add("idSubElement", idSubElement.ToString());
            }
            if (idSupervisor is not null)
            {
                parameters.Add("idSupervisor", idSupervisor);
            }

            return await _repository.GetAsync<List<ProgressReport>>(parameters, path: "api/ProgressReport"); ;
        }
        public async Task<List<AparmentProgress>?> GetProgresReportViewAsync(int? id)
        {
            Dictionary<string, string> parameters = new();
            parameters.Add("id", id.ToString());            
            return await _repository.GetAsync<List<AparmentProgress>?>(parameters, path: "api/ProgressReport/ProgressReportView");
        }
        public async Task<byte[]> PostProgressReporPDFtAsync(List<AparmentProgress> progressReportList)
        {            
            //Dictionary<string, List<AparmentProgress>> parameters = new();
            //parameters.Add("progressAparmentList", progressReportList);
            return await _repository.PostAsync<byte[]>(progressReportList, path: "api/ProgressReport/AparmentProgressPDF");
        }
        public async Task<ProgressReport> PostProgressReportAsync(ProgressReport progressReport)
        {
            return await _repository.PostAsync(progressReport, path: "api/ProgressReport");
        }
    }
}
