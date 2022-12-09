﻿using SharedLibrary.Models;
using SharedLibrary;
using ApiGateway.Proxies;
using ApiGateway.Interfaces;
using System.Xml.Linq;

namespace ApiGateway.Services
{
    public class ProgressReportService : GenericProxy, IProgressReportService
    {
        public ProgressReportService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<ProgressReport>> GetProgressReportAsync(int id)
        {
            return await GetAsync<ProgressReport>(id, path: "ProgressReport");
        }

        public async Task<ApiResponse<List<ProgressReport>>> GetProgressReportsAsync(int? idProgressReport, int? idBuilding, int? idAparment, int? idArea, int? idElemnet, int? idSubElement, string? idSupervisor)
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

            return await GetAsync<List<ProgressReport>>(path: "ProgressReports", parameters: parameters);
        }

        public async Task<ApiResponse<List<AparmentProgress>>?> GetProgressReportViewAsync(int? id)
        {
            Dictionary<string, string> parameters = new();
            if(id != null)
            {
                parameters.Add("idAparment", id.ToString());
            }            
            return await GetAsync<List<AparmentProgress>?>(path: "ReporteAvanceVista", parameters: parameters);
        }

        public async Task<ApiResponse<ProgressReport>> PostProgressReportAsync(ProgressReport progressReport)
        {
            return await PostAsync<ProgressReport>(progressReport, path: "ProgressReport");
        }
    }
}
