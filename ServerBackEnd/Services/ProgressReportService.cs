﻿using SharedLibrary.Models;
using SharedLibrary;
using ApiGateway.Proxies;
using ApiGateway.Interfaces;
using System.Xml.Linq;
using static OpenIddict.Abstractions.OpenIddictConstants;

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

        public async Task<ApiResponse<List<ProgressReport>>> GetProgressReportsAsync(int? idProgressReport, int? idBuilding, int? idApartment, int? idArea, int? idElement, int? idSubElement, string? idSupervisor, bool includeProgressLogs)
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

            return await GetAsync<List<ProgressReport>>(path: "ProgressReports", parameters: parameters);
        }

        public async Task<ApiResponse<ObjectAccessUser>> GetObjectAccessAsync(string idSupervisor)
        {
            return await GetAsync<ObjectAccessUser>(idSupervisor, path: "ObjectsAccess");
        }

        public async Task<ApiResponse<int>> GetBuildingAssignedAsync(string idSupervisor)
        {
            return await GetAsync<int>(idSupervisor, path: "BuildingAssigned");
        }

        public async Task<ApiResponse<ProgressReport>> PostProgressReportAsync(ProgressReport progressReport)
        {
            return await PostAsync<ProgressReport>(progressReport, path: "ProgressReport");
        }
    }
}
