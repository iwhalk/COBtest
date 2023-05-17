using Microsoft.EntityFrameworkCore;
using ReportesObra.Interfaces;
using SharedLibrary.Data;
using SharedLibrary.Models;
using SixLabors.ImageSharp.ColorSpaces;
using System.Linq;
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
            else
            {
                System.Linq.Expressions.Expression<Func<ProgressReport, ProgressReport>> selector2 = x => new ProgressReport
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
                    IdElementNavigation= x.IdElementNavigation
                };

                return await progressReports.Select(selector2).ToListAsync();
            }
        }

        public async Task<List<ProgressReport>?> GetProgressReportsDetailedAsync(int idBuilding, List<int>? idApartments,
            List<int>? idAreas, List<int>? idElements, List<int>? idSubElements, List<int>? idActivities)
        {
            IQueryable<ProgressReport> progressReports = _dbContext.ProgressReports;
            progressReports = progressReports.Where(x => x.IdBuilding == idBuilding);

            if (idApartments != null && idApartments.Count != 0)
                progressReports = progressReports.Where(x => idApartments.Contains(x.IdApartment));
            if (idAreas != null && idAreas.Count != 0)
                progressReports = progressReports.Where(x => idAreas.Contains(x.IdArea));
            if (idElements != null && idElements.Count != 0)
                progressReports = progressReports.Where(x => idElements.Contains(x.IdElement));
            if (idSubElements != null && idSubElements.Count != 0)
                progressReports = progressReports.Where(x => idSubElements.Contains(x.IdSubElement ?? 0));

            System.Linq.Expressions.Expression<Func<Element, Element>> selectorE = e => new Element
            {
                IdElement = e.IdElement,
                ElementName = e.ElementName,
                IdActivity = e.IdActivity,
                IdActivityNavigation = e.IdActivityNavigation
            };

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
                IdApartmentNavigation = x.IdApartmentNavigation,
                IdAreaNavigation = x.IdAreaNavigation,
                IdElementNavigation = _dbContext.Elements.Select(selectorE).FirstOrDefault(a => a.IdElement == x.IdElement),
                IdSubElementNavigation = x.IdSubElementNavigation,
                TimePiece = x.TimePiece,
                CostPiece = x.CostPiece,
                ProgressLogs = x.ProgressLogs.Select(y => new ProgressLog
                {
                    IdProgressLog = y.IdProgressLog,
                    IdProgressReport = y.IdProgressReport,
                    DateCreated = y.DateCreated,
                    IdStatus = y.IdStatus,
                    Pieces = y.Pieces,
                    Observation = y.Observation,
                    IdSupervisor = y.IdSupervisor,
                    IdStatusNavigation = y.IdStatusNavigation,
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
            var result = progressReports.Select(selector);
            if (idActivities != null && idActivities.Count != 0)
                result = result.Where(x => idActivities.Contains(x.IdElementNavigation.IdActivity));            
            return await result.ToListAsync();
        }

        public async Task<ObjectAccessUser?> GetObjectsAccessAsync(string idSupervisor)
        {
            IQueryable<ProgressReport> progressReports = _dbContext.ProgressReports;
            //Se filtra por el ID del AspNetUser para obtener los elementos a los que tiene acceso
            progressReports = progressReports.Where(x => x.IdSupervisor == idSupervisor).OrderBy(x => x.IdApartment);
            System.Linq.Expressions.Expression<Func<Element, Element>> selector = x => new Element
            {
                IdElement = x.IdElement,
                ElementName = x.ElementName,
                IdActivity = x.IdActivity,
                Type = x.Type
            };

            //Se obtienen las listas de Id's de de los campos que se usan en la vista de Seguimiento del Proyecto
            var idApartments = progressReports.GroupBy(a => a.IdApartment).Select(b => b.First().IdApartment);
            //var idApartments = progressReports.Select(x => x.IdApartment).Distinct();
            var idAreas = progressReports.GroupBy(a => a.IdArea).Select(b => b.First().IdArea);            
            var idElements = progressReports.GroupBy(a => a.IdElement).Select(b => b.First().IdElement);
            var idSubelements = progressReports.GroupBy(a => a.IdSubElement).Select(b => b.First().IdSubElement);
            //El selector para Elements evita que se ciclen las referencias a Activities
            var elements0 = _dbContext.Elements.Select(selector);
            //Obtenemos de una vez los objectos de Elements y la lista de ID'ss de Activities ya que ID_Activity no viene referenciado en la tabla de ProgressReport
            var elements = elements0.Where(x => idElements.Contains(x.IdElement));
            var idActivities = elements.GroupBy(a => a.IdActivity).Select(b => b.First().IdActivity);
            //Calculando la moda con Entity Framework
            var idBuildingFound = progressReports.GroupBy(x => x.IdBuilding)
                .OrderByDescending(x => x.Count()).ThenBy(x => x.Key)
                .Select(x => (int?)x.Key)
                .FirstOrDefault();
            //var q = progressReports.Select(x => x.IdBuilding).Distinct();

            ObjectAccessUser list = new ObjectAccessUser()
            {
                IdBuilding = idBuildingFound ?? 0,
                Apartments = _dbContext.Apartments.Where(x => idApartments.Contains(x.IdApartment)).OrderBy(x => x.IdApartment).ToList(),
                Areas = _dbContext.Areas.Where(x => idAreas.Contains(x.IdArea)).OrderBy(x => x.IdArea).ToList(),
                Activities = _dbContext.Activities.Where(x => idActivities.Contains(x.IdActivity)).OrderBy(x => x.IdActivity).ToList(),
                Elements = elements.OrderBy(x => x.IdElement).ToList(),
                SubElements = _dbContext.SubElements.Where(x => idSubelements.Contains(x.IdSubElement)).OrderBy(x => x.IdSubElement).ToList(),
            };

            return list;
        }

        public async Task<int> GetIdBuildingAssigned(string idSupervisor)
        {
            IQueryable<ProgressReport> progressReports = _dbContext.ProgressReports;
            //Se filtra por el ID del AspNetUser para obtener los elementos a los que tiene acceso
            progressReports = progressReports.Where(x => x.IdSupervisor == idSupervisor);

            var idBuildingFound = progressReports.GroupBy(x => x.IdBuilding)
                .OrderByDescending(x => x.Count()).ThenBy(x => x.Key)
                .Select(x => (int?)x.Key)
                .FirstOrDefault();

            return idBuildingFound ?? 0;
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

