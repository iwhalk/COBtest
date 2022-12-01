using Microsoft.EntityFrameworkCore;
using ReportesObra.Interfaces;
using SharedLibrary.Data;
using SharedLibrary.Models;
using System.Net.NetworkInformation;

namespace ReportesObra.Services
{
    public class ProgressReportsService : IProgressReportsService
    {
        private readonly ObraDbContext _dbContext;

        public ProgressReportsService(ObraDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<ProgressReport>?> GetProgressReportsAsync(int? idProgressReport, int? idBuilding, int? idAparment, int? idArea, int? idElemnet, int? idSubElement, string? idSupervisor)
        {
            IQueryable<ProgressReport> progressReports = _dbContext.ProgressReports;

            if (idProgressReport != null)
                progressReports = progressReports.Where(x => x.IdProgressReport == idProgressReport);
            if (idBuilding != null)
                progressReports = progressReports.Where(x => x.IdBuilding == idBuilding);
            if (idAparment != null)
                progressReports = progressReports.Where(x => x.IdApartment == idAparment);
            if (idArea != null)
                progressReports = progressReports.Where(x => x.IdArea == idArea);
            if (idElemnet != null)
                progressReports = progressReports.Where(x => x.IdElement == idElemnet);
            if (idSubElement != null)
                progressReports = progressReports.Where(x => x.IdSubElement == idSubElement);
            if (idSupervisor != null)
                progressReports = progressReports.Where(x => x.IdSupervisor == idSupervisor);

            return progressReports.ToListAsync();
        }

        public async Task<ProgressReport?> CreateProgressReportsAsync(ProgressReport progressReport)
        {
            await _dbContext.ProgressReports.AddAsync(progressReport);
            try { await _dbContext.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }
            return progressReport;
        }

        public async Task<bool> UpdateProgressReportsAsync(ProgressReport progressReport)
        {
            _dbContext.Entry(progressReport).State = EntityState.Modified;
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

