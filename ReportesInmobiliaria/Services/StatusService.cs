using Microsoft.EntityFrameworkCore;
using ReportesObra.Interfaces;
using SharedLibrary.Data;
using SharedLibrary.Models;
using System.Reflection.Metadata.Ecma335;

namespace ReportesObra.Services
{
    public class StatusService : IStatusService
    {
        private readonly ObraDbContext _dbContext;

        public StatusService(ObraDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Status>?> GetStatusesAsync()
        {
            return await _dbContext.Statuses.ToListAsync();
        }
        public async Task<Status?> GetStatusAsync(int idStatus)
        {
            return await _dbContext.Statuses.FirstOrDefaultAsync(x => x.IdStatus == idStatus);
        }
    }
}
