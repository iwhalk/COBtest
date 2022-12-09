﻿using SharedLibrary;
using SharedLibrary.Models;

namespace ApiGateway.Interfaces
{
    public interface IProgressReportService
    {
        Task<ApiResponse<List<AparmentProgress>>?> GetProgressReportViewAsync(int? id);
        Task<ApiResponse<ProgressReport>> GetProgressReportAsync(int id);
        Task<ApiResponse<List<ProgressReport>>> GetProgressReportsAsync(int? idProgressReport, int? idBuilding, int? idAparment, int? idArea, int? idElemnet, int? idSubElement, string? idSupervisor);
        Task<ApiResponse<ProgressReport>> PostProgressReportAsync(ProgressReport progressReport);
    }
}
