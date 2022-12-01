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

        public async Task<List<ProgressLog>?> GetProgressLogAsync(int? idProgressLog, int? idProgressReport, int? idStatus, string? idSupervisor)
        {
            IQueryable<ProgressLog> progressLogs = _dbContext.ProgressLogs;

            if (idProgressLog != null)
                progressLogs = progressLogs.Where(x => x.IdProgressLog == idProgressLog);
            if (idProgressReport != null)
                progressLogs = progressLogs.Where(x => x.IdProgressReport == idProgressReport);
            if (idStatus != null)
                progressLogs = progressLogs.Where(x => x.IdStatus == idStatus);
            if (idSupervisor != null)
                progressLogs = progressLogs.Where(x => x.idSupervisor == idSupervisor);

            return progressLogs;
        }

        public async Task<Area?> CreateAreaAsync(ProgressLog progressLog)
        {
            await _dbContext.ProgressLogs.AddAsync(progressLog);
            try { await _dbContext.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }
            return progressLog;
        }

        public async Task<bool> UpdateAreaAsync(Area area)
        {
            _dbContext.Entry(area).State = EntityState.Modified;
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

        public async Task<bool> DeleteAreaAsync(int id)
        {
            Area? area = _dbContext.Areas.FirstOrDefault(x => x.IdArea == id);
            if (area == null)
                return false;
            _dbContext.Areas.Remove(area);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
