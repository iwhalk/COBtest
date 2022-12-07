﻿using SharedLibrary.Models;
using SharedLibrary;

namespace Obra.Client.Interfaces
{
    public interface IProgressLogsService
    {
        Task<List<ProgressLog>> GetProgressLogsAsync(int? idProgressLog, int? idProgressReport, int? idStatus, string? idSupervisor);
        Task<ProgressLog> PostProgressLogAsync(ProgressLog progressLog);
    }
}
