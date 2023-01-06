using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using ReportesObra.Interfaces;
using ReportesObra.Utilities;
using SharedLibrary.Data;
using SharedLibrary.Models;
using StoredProcedureEFCore;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
//using System.Data.Entity;
//using System.Data.Entity;
using System.Data.Entity.Infrastructure;
//using System.Diagnostics;
using System.Linq;

namespace ReportesObra.Services
{
    public class ReportsService : IReportesService
    {
        private readonly ObraDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ReportesFactory _reportesFactory;
        List<ProgressReport> progressReportsComplete;
        List<ProgressLog> listProgressLog;
        List<Element> listElements;
        List<SharedLibrary.Models.Activity> listActivities;
        List<SubElement> listSubElements;
        List<Apartment> listApartments;
        //private readonly InmobiliariaDbContextProcedures _dbContextProcedure;

        public ReportsService(ObraDbContext dbContext, IHttpContextAccessor httpContextAccessor, ReportesFactory reportesFactory)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _reportesFactory = reportesFactory;
            progressReportsComplete = _dbContext.ProgressReports.ToList();
            listProgressLog = _dbContext.ProgressLogs.ToList();
            listElements = _dbContext.Elements.ToList();
            listActivities = _dbContext.Activities.ToList();
            listSubElements = _dbContext.SubElements.ToList();
            listApartments = _dbContext.Apartments.ToList();
        }

        public async Task<byte[]> GetReporteDetalles(int idBuilding, List<int>? idApartments, List<int>? idActivities, List<int>? idElements, List<int>? idSubElements)
        {
            List<ProgressReport> listReport = new List<ProgressReport>();
            string title = "(Todos)";
            listReport = progressReportsComplete.Where(x => x.IdBuilding == idBuilding).ToList();
            if (idApartments != null && idApartments.Count() != 0)
            {
                listReport = FiltradoIdApartments(listReport, idApartments);
                title = "(Seleccionados)";
            }
            if (idElements != null && idElements.Count() != 0)
                listReport = FiltradoIdElements(listReport, idElements);
            if (idSubElements != null && idSubElements.Count() != 0)
                listReport = FiltradoIdSubElements(listReport, idSubElements);

            ReporteDetalles reporteDetalles = new()
            {
                detalladoActividades = GetSubElementsAsync(listReport)
            };
            if(idActivities != null && idActivities.Count() != 0)
                reporteDetalles.detalladoActividades = FiltradoIdActivities(reporteDetalles.detalladoActividades, idActivities);
            if (reporteDetalles.detalladoActividades.Count == 0)
                return null;                
            return _reportesFactory.CrearPdf(reporteDetalles, title);
        }

        public async Task<byte[]?> GetReporteDetallesActividad(int idBuilding, List<int>? idActivities, List<int>? idElements, List<int>? idSubElements, List<int>? idApartments)
        {
            List<ProgressReport> listReport = new List<ProgressReport>();
            string title = "(Todas)";
            listReport = progressReportsComplete.Where(x => x.IdBuilding == idBuilding).ToList();
            if(idElements != null && idElements.Count() != 0)
                listReport = FiltradoIdElements(listReport, idElements);
            if (idSubElements != null && idSubElements.Count() != 0)
                listReport = FiltradoIdSubElements(listReport, idSubElements);
            if (idApartments != null && idApartments.Count() != 0)
                listReport = FiltradoIdApartments(listReport, idApartments);
            ReporteDetalles reporteDetalles = new()
            {
                detalladoActividades = GetSubElementsAsync(listReport)
            };
            if (idActivities != null && idActivities.Count() != 0){
                reporteDetalles.detalladoActividades = FiltradoIdActivities(reporteDetalles.detalladoActividades, idActivities);
                title = "(Seleccionadas)";
            }

            if (reporteDetalles.detalladoActividades.Count == 0)
                return null;
            return _reportesFactory.CrearPdf(reporteDetalles, title);
        }

        public List<DetalladoActividades> GetSubElementsAsync(List<ProgressReport> progressList)
        {
            int contador = 0;
            var list = new List<DetalladoActividades>();                       
            foreach (var subElement in progressList)
            {
                if (getStausName(subElement.IdProgressReport) == "Terminado")
                {
                    contador++;
                    continue;
                }
                    
                list.Add(new DetalladoActividades()
                {
                    numeroApartamento = getApartmentNumber(subElement.IdApartment),
                    actividad = getActividadByElement(subElement.IdElement),
                    elemento = getElemento(subElement.IdElement),
                    subElemento = getSubElemento(subElement.IdSubElement),
                    estatus = getStausName(subElement.IdProgressReport),
                    total = subElement.TotalPieces,
                    avance = getProgress(subElement.IdProgressReport),
                });
            }
            return list;
        }
        public async Task<byte[]> GetReporteAvance(List<AparmentProgress> aparmentProgress)
        {
            ReporteAvance reporteAvance = new()
            {
                FechaGeneracion = DateTime.Now,
                Apartments = aparmentProgress
            };
            return _reportesFactory.CrearPdf(reporteAvance);
        }

        public async Task<ReporteAvance> GetReporteAvanceVista(int? idAparment)
        {
            ReporteAvance reporteAvance = new()
            {
                FechaGeneracion = DateTime.Now,
                Apartments = GetAparmentsAsync(idAparment)
            };
            return reporteAvance;
        }

        public List<AparmentProgress> GetAparmentsAsync(int? idAparment)
        {
            IQueryable<ProgressReport> progressReports = _dbContext.ProgressReports;
            IQueryable<ProgressLog> progressLogs= _dbContext.ProgressLogs;
            IQueryable<Apartment> apartments = _dbContext.Apartments;

            if (idAparment != null)
                progressReports = progressReports.Where(x => x.IdApartment == idAparment);

            List<TotalPicesByAparment> totalOfPicesByAparment = progressReports.GroupBy(x => x.IdApartment)
                    .Select(x => new TotalPicesByAparment
                    {
                        IdAparment = x.Key,
                        Pices = x.Sum(s => Convert.ToInt32(s.TotalPieces))
                    }).ToList();

            var progressReportsByAparment = progressReports.Join(progressLogs, x => x.IdProgressReport, y => y.IdProgressReport, (report, log) => new {report, log}).GroupBy(x => x.report.IdApartment);

            var list = new List<AparmentProgress>();
            foreach (var aparment in progressReportsByAparment)
            {                
                long total = totalOfPicesByAparment.FirstOrDefault(x => x.IdAparment == aparment.Key).Pices;
                long current = aparment.GroupBy(x => x.log.IdProgressReport).Select(x => x.OrderByDescending(x => x.log.DateCreated).FirstOrDefault()).Sum(x => long.Parse(x.log.Pieces));
                list.Add(new AparmentProgress()
                {
                    ApartmentNumber = apartments.FirstOrDefault(x => x.IdApartment == aparment.Key).ApartmentNumber,
                    ApartmentProgress = 100.00 / total * current
                });
            }
            return list;
        }

        public async Task<List<AparmentProgress>> GetAparments(int? idAparment)
        {
            IQueryable<ProgressReport> progressReports = _dbContext.ProgressReports;
            IQueryable<ProgressLog> progressLogs = _dbContext.ProgressLogs;
            //IQueryable<ProgressLog> progressLogsMaxDate = _dbContext.ProgressLogs.GroupBy(x => x.IdProgressReport).Select(x => x.);
            IQueryable<Apartment> apartments = _dbContext.Apartments;

            if (idAparment != null)
                progressReports = progressReports.Where(x => x.IdApartment == idAparment);

            progressReports = progressReports.Where(x => x.IdBuilding == 1);

            List<TotalPicesByAparment> totalOfPicesByAparment = progressReports.GroupBy(x => x.IdApartment)
                    .Select(x => new TotalPicesByAparment
                    {
                        IdAparment = x.Key,
                        Pices = x.Sum(s => Convert.ToInt32(s.TotalPieces))
                    }).ToList();

            var progressReportsByAparment = progressReports.Join(progressLogs, x => x.IdProgressReport, y => y.IdProgressReport, (report, log) => new { report, log }).GroupBy(x => x.report.IdApartment);

            var list = new List<AparmentProgress>();
            foreach (var aparment in progressReportsByAparment)
            {
                long total = totalOfPicesByAparment.FirstOrDefault(x => x.IdAparment == aparment.Key).Pices;
                long current = aparment.GroupBy(x => x.log.IdProgressReport).Select(x => x.OrderByDescending(x => x.log.DateCreated).FirstOrDefault()).Sum(x => long.Parse(x.log.Pieces));
                list.Add(new AparmentProgress()
                {
                    ApartmentNumber = apartments.FirstOrDefault(x => x.IdApartment == aparment.Key).ApartmentNumber,
                    ApartmentProgress = 100.00 / total * current
                });
            }
            return list;
        }
        //public async Task<byte[]> GetActivityProgress(int idBuilding, List<int>? idActivities)
        public async Task<List<ActivityProgress>> GetActivityProgress(int? idBuilding, int? idActivity)
        {
            if (idBuilding == null)
                idBuilding = 1;
            List<ProgressReportActivityAdded> progressReportsWithActivity = new List<ProgressReportActivityAdded>();
            IQueryable<ProgressReport> progressReports = _dbContext.ProgressReports;
            IQueryable<ProgressLog> progressLogs = _dbContext.ProgressLogs;
            IQueryable<SharedLibrary.Models.Activity> activities = _dbContext.Activities;
            progressReports = progressReports.Where(x => x.IdBuilding == idBuilding);            

            foreach (var progressReport in progressReports)
            {
                progressReportsWithActivity.Add(new ProgressReportActivityAdded()
                {
                    IdProgressReport = progressReport.IdProgressReport,
                    DateCreated = progressReport.DateCreated,
                    IdApartment = progressReport.IdApartment,
                    IdArea = progressReport.IdArea,
                    IdActivity = getIdActividadByElement(progressReport.IdElement),
                    IdElement = progressReport.IdElement,
                    TotalPieces = progressReport.TotalPieces
                });
            }
            if (idActivity != null)
                progressReportsWithActivity = progressReportsWithActivity.Where(x => x.IdActivity == idActivity).ToList();

            List<TotalPiecesByActivity> totalPieces = progressReportsWithActivity.GroupBy(x => x.IdActivity)
                    .Select(x => new TotalPiecesByActivity
                    {
                        IdActivity = (int)x.Key,
                        Pieces = x.Sum(s => Convert.ToInt32(s.TotalPieces))
                    }).ToList();

            var progressReportsByActivity = progressReportsWithActivity.Join(progressLogs, x => x.IdProgressReport, y => y.IdProgressReport,
                (report, log) => new { report, log }).GroupBy(x => x.report.IdActivity);
            var list = new List<ActivityProgress>();
            foreach (var activity in progressReportsByActivity)
            {
                long total = totalPieces.FirstOrDefault(x => x.IdActivity == activity.Key).Pieces;
                long current = activity.GroupBy(x => x.log.IdProgressReport).Select(x => x.OrderByDescending(x => x.log.DateCreated).FirstOrDefault()).Sum(x => long.Parse(x.log.Pieces));
                list.Add(new ActivityProgress()
                {
                    ActivityName = activities.FirstOrDefault(x => x.IdActivity == activity.Key).ActivityName,
                    Progress = 100.00 / total * current
                });
            }
            //list = list.OrderBy(x => x.ActivityName).ToList();
            return list;
            //ReporteAvanceActividad reporteAvance = new()
            //{
            //    FechaGeneracion = DateTime.Now,
            //    Activities = list
            //};
            //return _reportesFactory.CrearPdf(reporteAvance, title);
        }

        public async Task<byte[]> GetReporteAvanceActividad(List<ActivityProgress> activityProgress)
        {
            string reportTitle = "";
            List<string> activityNames = new List<string>();
            foreach (var activity in activityProgress)
                activityNames.Add(activity.ActivityName);
            foreach (var activity in listActivities)
            {
                if (activityNames.Contains(activity.ActivityName))
                    reportTitle = "(Todas)";
                else
                {
                    reportTitle = "(Seleccionadas)";
                    break;
                }
            }

            var name = listActivities.ElementAt(0).ActivityName;
            ReporteAvanceActividad reporteAvance = new()
            {
                FechaGeneracion = DateTime.Now,
                Activities = activityProgress
            };
            return _reportesFactory.CrearPdf(reporteAvance, reportTitle);
        }

        public int? getIdActividadByElement(int idElement)
        {
            var localElement = listElements.FirstOrDefault(x => x.IdElement == idElement);
            if (localElement == null)
                return null;
            var nombreActividad = listActivities.FirstOrDefault(x => x.IdActivity == localElement.IdActivity);
            return nombreActividad == null ? null : nombreActividad.IdActivity;
        }

        public async Task<List<AparmentProgress>> GetActivitiesByAparment(int? idAparment)
        {
            IQueryable<ProgressReport> progressReports = _dbContext.ProgressReports.Include(x => x.IdApartmentNavigation).Include(x => x.IdElementNavigation).Include(x => x.IdElementNavigation.IdActivityNavigation);
            IQueryable<Activity> Activities = _dbContext.Activities;
            IQueryable<ProgressLog> progressLogs = _dbContext.ProgressLogs;                              
            var list = new List<AparmentProgress>();

            progressReports = progressReports.Where(x => x.IdBuilding == 1);

            foreach(var activity in Activities)
            {
                var progressReportsCurrentActivity = progressReports.Where(x => x.IdElementNavigation.IdActivityNavigation.IdActivity.Equals(activity.IdActivity));

                List<TotalPicesByAparment> totalOfPicesByAparment = progressReportsCurrentActivity.GroupBy(x => x.IdApartment)
                    .Select(x => new TotalPicesByAparment
                    {
                        IdAparment = x.Key,
                        Pices = x.Sum(s => Convert.ToInt32(s.TotalPieces))
                    }).ToList();

                var progressReportsCurrentActivityByAparment = progressReportsCurrentActivity.Join(progressLogs, x => x.IdProgressReport, y => y.IdProgressReport, (report, log) => new { report, log }).GroupBy(x => x.report.IdApartment);

                foreach (var apartment in progressReportsCurrentActivityByAparment)
                {
                    long total = totalOfPicesByAparment.FirstOrDefault(x => x.IdAparment == apartment.Key).Pices;
                    long current;
                    if (apartment != null)
                        current = apartment.GroupBy(x => x.log.IdProgressReport).Select(x => x.OrderByDescending(x => x.log.DateCreated).FirstOrDefault()).Sum(x => long.Parse(x.log.Pieces));
                    else
                        current = 99999999999999;
                    list.Add(new AparmentProgress()
                    {
                        Activity_ = apartment.FirstOrDefault().report.IdElementNavigation.IdActivityNavigation.ActivityName,
                        ApartmentNumber = apartment.FirstOrDefault().report.IdApartmentNavigation.ApartmentNumber,
                        ApartmentProgress = 100.00 / total * current
                    });
                }

            }            
            //var listGroupedByActivity = list.GroupBy(x => x.Activity_).ToList();
            return list;
        }

        public async Task<byte[]> GetReporteAvancDeActividadPorDepartamento(List<AparmentProgress> aparmentProgress)
        {
            var listGroupedByActivity = aparmentProgress.GroupBy(x => x.Activity_).ToList();

            ReporteAvance reporteAvance = new()
            {
                FechaGeneracion = DateTime.Now,
                Apartments = aparmentProgress
            };
            return _reportesFactory.CrearPdf(listGroupedByActivity);
        }

        public string? getActividadByElement(int idElement)
        {
            var localElement = listElements.FirstOrDefault(x => x.IdElement == idElement);
            if (localElement == null)
                return null;
            var nombreActividad = listActivities.FirstOrDefault(x => x.IdActivity == localElement.IdActivity);
            return nombreActividad == null ? null : nombreActividad.ActivityName;            
        }

        private string? getActividad(int idActividad)
        {
            var nombreActividad = listActivities.FirstOrDefault(x => x.IdActivity == idActividad);
            return nombreActividad == null ? null : nombreActividad.ActivityName;
        }

        public string? getElemento(int? idElement)
        {
            var nameElement = listElements.FirstOrDefault(x => x.IdElement == idElement);
            if (nameElement == null)
                return null;
            return nameElement.ElementName;
        }

        public string? getSubElemento(int? idSubElement)
        {
            if (idSubElement == null)
                return "N/A";
            var nameSubElement = listSubElements.FirstOrDefault(x => x.IdSubElement == idSubElement);
            if (nameSubElement == null)
                return null;
            return nameSubElement.SubElementName;
        }

        public string getStausName(int idProgressReport)
        {
            var result = listProgressLog.LastOrDefault(x => x.IdProgressReport == idProgressReport);
            if (result == null)
                return "Pendiente";
            int? idStatus = result.IdStatus;
            switch (idStatus)
            {
                case 1:
                    return "Pendiente";
                case 2:
                    return "En curso";
                case 3:
                    return "Terminado";
                default:
                    return "Pendiente";
            }
        }

        private int getProgress(int idProgressReport)
        {
            int i = 0;
            var result = listProgressLog.LastOrDefault(x => x.IdProgressReport == idProgressReport);
            if (result == null)
                return 0;
            string pieces = result.Pieces;
            bool canConvert = Int32.TryParse(pieces, out i);
            return i;
        }

        private string getApartmentNumber(int idApartment)
        {
            var result = listApartments.FirstOrDefault(x => x.IdApartment == idApartment);
            if (result == null)
                return null;
            return result.ApartmentNumber;
        }

        private List<ProgressReportActivityAdded> FiltradoProgressByActivity(List<ProgressReportActivityAdded> listProgress, List<int> idActivities)
        {
            List<ProgressReportActivityAdded> listProgressFiltred = new List<ProgressReportActivityAdded>();
            foreach (var idActivity in idActivities)
            {
                var subListActivity = listProgress.Where(x => x.IdActivity == idActivity);
                if (subListActivity == null)
                    continue;
                listProgressFiltred.AddRange(subListActivity.ToList());
            }
            return listProgressFiltred;
        }

        private List<ProgressReport> FiltradoIdApartments(List<ProgressReport>  ListAllAparments, List<int> idApartments)
        {
            List<ProgressReport> apartmentsFiltred = new List<ProgressReport>();
            foreach (var idApartment in idApartments)
            {
                var subListApartments = ListAllAparments.Where(x => x.IdApartment == idApartment);
                if (subListApartments == null)
                    continue;
                apartmentsFiltred.AddRange(subListApartments.ToList());
            }
            return apartmentsFiltred;
        }

        private List<DetalladoActividades> FiltradoIdActivities(List<DetalladoActividades> ListAllActivities, List<int> idActivities)
        {
            List<string> activityNames = new List<string>();
            List<DetalladoActividades> activitiesFiltred = new List<DetalladoActividades>();
            foreach (var idActivity in idActivities)
            {
                string activityName = getActividad(idActivity);
                if (activityName == null)
                    continue;
                activityNames.Add(activityName);
            }

            foreach (var activityName in activityNames)
            {
                var subListActivities = ListAllActivities.Where(x => x.actividad == activityName);
                if (subListActivities == null)
                    continue;
                activitiesFiltred.AddRange(subListActivities.ToList());
            }
            return activitiesFiltred;
        }

        private List<ProgressReport> FiltradoIdElements(List<ProgressReport> ListAllElements, List<int> idElements)
        {
            List<ProgressReport> elementsFiltred = new List<ProgressReport>();
            foreach (var idElement in idElements)
            {
                var subListElements = ListAllElements.Where(x => x.IdElement == idElement);
                if (subListElements== null)
                    continue;
                elementsFiltred.AddRange(subListElements.ToList());
            }
            return elementsFiltred;
        }

        private List<ProgressReport> FiltradoIdSubElements(List<ProgressReport> ListAllSubElements, List<int> idSubElements)
        {
            List<ProgressReport> subElementsFiltred = new List<ProgressReport> ();
            foreach (var idSubElement in idSubElements)
            {
                var subListSubElements = ListAllSubElements.Where(x => x.IdSubElement == idSubElement);
                if(subListSubElements == null)
                    continue;
                subElementsFiltred.AddRange(subListSubElements.ToList());
            }
            return subElementsFiltred;
        }
    }
}