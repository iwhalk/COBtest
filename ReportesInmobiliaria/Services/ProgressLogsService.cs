using Microsoft.EntityFrameworkCore;
using ReportesObra.Interfaces;
using SharedLibrary.Data;
using SharedLibrary.Models;

namespace ReportesObra.Services
{
    public class ProgressLogsService : IProgressLogsService
    {
        private readonly ObraDbContext _dbContext;

        public ProgressLogsService(ObraDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProgressLog> GetProgressLogAsync(int idProgressLog)
        {
            return await _dbContext.ProgressLogs.FirstOrDefaultAsync(x => x.IdProgressLog == idProgressLog);
        }

        public Task<List<ProgressLog>?>? GetProgressLogsAsync(int? idProgressLog, int? idProgressReport, int? idStatus, string? idSupervisor)
        {
            IQueryable<ProgressLog> progressLogs = _dbContext.ProgressLogs;

            if (idProgressLog != null)
                progressLogs = progressLogs.Where(x => x.IdProgressLog == idProgressLog);
            if (idProgressReport != null)
                progressLogs = progressLogs.Where(x => x.IdProgressReport == idProgressReport);
            if (idStatus != null)
                progressLogs = progressLogs.Where(x => x.IdStatus == idStatus);
            if (idSupervisor != null)
                progressLogs = progressLogs.Where(x => x.IdSupervisor == idSupervisor);

            System.Linq.Expressions.Expression<Func<ProgressLog, ProgressLog>> selector = x => new ProgressLog
            {
                IdProgressLog = x.IdProgressLog,
                IdProgressReport = x.IdProgressReport,
                DateCreated = x.DateCreated,
                IdStatus = x.IdStatus,
                Pieces = x.Pieces,
                Observation = x.Observation,
                IdSupervisor = x.IdSupervisor,
                IdBlobs = x.IdBlobs.Select(y => new Blob
                {
                    IdBlob = y.IdBlob,
                    ContainerName = y.ContainerName,
                    IsPrivate = y.IsPrivate,
                    Uri = y.Uri,
                    BlobSize = y.BlobSize
                }).ToList()
            };
            return progressLogs?.Select(selector).ToListAsync();
        }

        public async Task<ProgressLog?> CreateProgressLogsAsync(ProgressLog progressLog)
        {
            if (progressLog.IdBlobs != null)
                foreach (var blob in progressLog.IdBlobs)
                {
                    _dbContext.Attach(blob);
                }
            await _dbContext.ProgressLogs.AddAsync(progressLog);
            try { await _dbContext.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }
            return progressLog;
        }

        public async Task<bool> UpdateProgressLogsAsync(ProgressLog progressLog)
        {
            _dbContext.Entry(progressLog).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return true;
        }
    }
}
