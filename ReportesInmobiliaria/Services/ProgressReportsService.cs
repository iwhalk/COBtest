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

        public async Task<ProgressReport> GetProgressReportAsync(int idProgressReport)
        {
            return await _dbContext.ProgressReports.FirstOrDefaultAsync(x => x.IdProgressReport == idProgressReport);
        }

        public async Task<List<ProgressReport>?> GetProgressReportsAsync(int? idProgressReport, int? idBuilding, int? idApartment, int? idArea, int? idElement, int? idSubElement, string? idSupervisor, bool includeProgressLogs)
        {
            IQueryable<ProgressReport> progressReports = _dbContext.ProgressReports;

            if (idProgressReport != null)
                progressReports = progressReports.Where(x => x.IdProgressReport == idProgressReport);
            if (idBuilding != null)
                progressReports = progressReports.Where(x => x.IdBuilding == idBuilding);
            if (idApartment != null)
                progressReports = progressReports.Where(x => x.IdApartment == idApartment);
            if (idArea != null)
                progressReports = progressReports.Where(x => x.IdArea == idArea);
            if (idElement != null)
                progressReports = progressReports.Where(x => x.IdElement == idElement);
            if (idSubElement != null)
                progressReports = progressReports.Where(x => x.IdSubElement == idSubElement);
            if (idSupervisor != null)
                progressReports = progressReports.Where(x => x.IdSupervisor == idSupervisor);

            if (includeProgressLogs == true)
            {
                System.Linq.Expressions.Expression<Func<ProgressReport, ProgressReport>> selector = x => new ProgressReport
                {
                    IdProgressReport = x.IdProgressReport,
                    DateCreated = x.DateCreated,
                    IdBuilding = x.IdBuilding,
                    IdApartment = x.IdApartment,
                    IdArea = x.IdArea,
                    IdElement = x.IdElement,
                    IdSubElement = x.IdSubElement,
                    TotalPieces = x.TotalPieces,
                    IdSupervisor = x.IdSupervisor,
                    ProgressLogs = x.ProgressLogs.Select(y => new ProgressLog
                    {
                        IdProgressLog = y.IdProgressLog,
                        IdProgressReport = y.IdProgressReport,
                        DateCreated = y.DateCreated,
                        IdStatus = y.IdStatus,
                        Pieces = y.Pieces,
                        Observation = y.Observation,
                        IdSupervisor = y.IdSupervisor,
                        IdBlobs = y.IdBlobs.Select(z => new Blob
                        {
                            IdBlob = z.IdBlob,
                            ContainerName = z.ContainerName,
                            IsPrivate = z.IsPrivate,
                            Uri = z.Uri,
                            BlobSize = z.BlobSize
                        }).ToList()

                    }).ToList()
                };

                return await progressReports.Select(selector).ToListAsync();
            }

            return await progressReports.ToListAsync();
        }

        public async Task<ProgressReport?> CreateProgressReportAsync(ProgressReport progressReport)
        {
            await _dbContext.ProgressReports.AddAsync(progressReport);
            try { await _dbContext.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }
            return progressReport;
        }

        public async Task<bool> UpdateProgressReportAsync(ProgressReport progressReport)
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

