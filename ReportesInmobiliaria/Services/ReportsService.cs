﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
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
        private readonly IProgressLogsService _progressLogsService;
        private readonly IProgressReportsService _progressReportsService;
        private readonly ReportesFactory _reportesFactory;
        List<Area> listAreas;
        List<Element> listElements;
        List<SharedLibrary.Models.Activity> listActivities;
        List<SubElement> listSubElements;
        List<Apartment> listApartments;
        List<Status> listStatuses;
        private static string _titleDetails = "(All)";
        private string _status1 = "Not Started";
        private string _status2 = "Started";
        private string _status3 = "Finished";

        //private readonly InmobiliariaDbContextProcedures _dbContextProcedure;

        public ReportsService(ObraDbContext dbContext, IHttpContextAccessor httpContextAccessor, ReportesFactory reportesFactory, IProgressReportsService progressReportsService, IProgressLogsService progressLogsService)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _reportesFactory = reportesFactory;
            _progressReportsService = progressReportsService;
            _progressLogsService = progressLogsService;
            listAreas = _dbContext.Areas.ToList();
            listElements = _dbContext.Elements.ToList();
            listActivities = _dbContext.Activities.ToList();
            listSubElements = _dbContext.SubElements.ToList();
            listApartments = _dbContext.Apartments.ToList();
            listStatuses = _dbContext.Statuses.ToList();
            if (listStatuses != null)
            {
                _status1 = listStatuses.ElementAtOrDefault(0) == null ? _status1 : listStatuses.ElementAt(0).StatusName;
                _status2 = listStatuses.ElementAtOrDefault(1) == null ? _status2 : listStatuses.ElementAt(1).StatusName;
                _status3 = listStatuses.ElementAtOrDefault(2) == null ? _status3 : listStatuses.ElementAt(2).StatusName;
            }            
        }

        public async Task<List<DetalladoDepartamentos>> GetDataDetallesDepartamento(int idBuilding, List<int>? idApartments, List<int>? idAreas, List<int>? idActivities, List<int>? idElements, List<int>? idSubElements)
        {
            if (idApartments != null && idApartments.Count() != 0)
                _titleDetails = "(Seleccionados)";
            else
                _titleDetails = "(Todos)";

            List<ProgressReport> listReport = new List<ProgressReport>();
            listReport = await _progressReportsService.GetProgressReportsDetailedAsync(idBuilding, idApartments, idAreas, idElements, idSubElements, idActivities);
            
            listReport = listReport.OrderBy(x => x.IdApartment).ThenBy(x => x.IdArea).ToList();
            var list = new List<DetalladoDepartamentos>();
            list = GetDetalladoDepartamentos(listReport);

            return list;
        }

        public async Task<byte[]> GetReporteDetallesDepartamento(List<DetalladoDepartamentos> detalladoDepartamentos, int? opcion)
        {
            if (opcion != null)
            {
                switch (opcion)
                {
                    case 1:
                        detalladoDepartamentos = detalladoDepartamentos.Where(x => x.estatus == _status1).ToList();
                        break;
                    case 2:
                        detalladoDepartamentos = detalladoDepartamentos.Where(x => x.estatus == _status2).ToList();
                        break;
                    case 3:
                        detalladoDepartamentos = detalladoDepartamentos.Where(x => x.estatus == _status3).ToList();
                        break;
                    default:
                        break;
                }
            }
            ReporteDetalles reporteDetalles = new()
            {
                detalladoDepartamentos = detalladoDepartamentos
            };
            if (reporteDetalles.detalladoDepartamentos.Count == 0)
                return null;
            return _reportesFactory.CrearPdf(reporteDetalles, _titleDetails);
        }

        public async Task<List<DetalladoActividades>> GetDataDetallesActividad(int idBuilding, List<int>? idActivities, List<int>? idElements, List<int>? idSubElements, List<int>? idApartments)
        {
            if (idActivities != null && idActivities.Count() != 0)
                _titleDetails = "(Seleccionados)";
            else
                _titleDetails = "(Todos)";

            List<ProgressReport> listReport = new List<ProgressReport>();
            listReport = await _progressReportsService.GetProgressReportsDetailedAsync(idBuilding, idApartments, null, idElements, idSubElements, idActivities);

            listReport = listReport.OrderBy(x => x.IdApartment).ThenBy(x => x.IdElement).ThenBy(x => x.IdArea).ToList();
            var list = new List<DetalladoActividades>();
            list = GetDetalladoActividades(listReport);

            return list;
        }

        public async Task<byte[]?> GetReporteDetallesActividad(List<DetalladoActividades> detalladoActividades, int? opcion)
        {
            if (opcion != null)
            {
                switch (opcion)
                {
                    case 1:
                        detalladoActividades = detalladoActividades.Where(x => x.estatus == _status1).ToList();
                        break;
                    case 2:
                        detalladoActividades = detalladoActividades.Where(x => x.estatus == _status2).ToList();
                        break;
                    case 3:
                        detalladoActividades = detalladoActividades.Where(x => x.estatus == _status3).ToList();
                        break;
                    default:
                        break;
                }
            }
            ReporteDetallesActividad reporteDetalles = new()
            {
                detalladoActividades = detalladoActividades
            };
            if (reporteDetalles.detalladoActividades.Count == 0)
                return null;
            return _reportesFactory.CrearPdf(reporteDetalles, _titleDetails);
        }

        public List<DetalladoDepartamentos> GetDetalladoDepartamentos(List<ProgressReport> progressList)
        {
            var list = new List<DetalladoDepartamentos>();
            foreach (var progress in progressList)
            {
                list.Add(new DetalladoDepartamentos()
                {
                    numeroApartamento = progress.IdApartmentNavigation.ApartmentNumber,
                    actividad = progress.IdElementNavigation.IdActivityNavigation.ActivityName,
                    area = progress.IdAreaNavigation.AreaName,
                    elemento = progress.IdElementNavigation.ElementName,
                    subElemento = progress.IdSubElementNavigation != null ? progress.IdSubElementNavigation.SubElementName : "N/A",
                    estatus = progress.ProgressLogs != null && progress.ProgressLogs.Count != 0 ? progress.ProgressLogs.Last().IdStatusNavigation.StatusName : _status1,
                    total = progress.TotalPieces,
                    avance = progress.ProgressLogs != null && progress.ProgressLogs.Count != 0 ? Int32.Parse(progress.ProgressLogs.Last().Pieces) : 0,
                    IdProgressLog = progress.ProgressLogs != null && progress.ProgressLogs.Count != 0 ? progress.ProgressLogs.Last().IdProgressLog : null,
                    HasObservationsOrBlobs = progress.ProgressLogs != null && progress.ProgressLogs.Count != 0 ? progressLogHasContent(progress.ProgressLogs) : false
                    //Obserbation = progress.ProgressLogs != null && progress.ProgressLogs.Count != 0 ? (progress.ProgressLogs.Last().Observation ?? null) : null,
                    //Blobs = getURIBlobs(progress.ProgressLogs)
                }) ;
            }
            return list;
        }

        public List<DetalladoActividades> GetDetalladoActividades(List<ProgressReport> progressList)
        {
            var list = new List<DetalladoActividades>();
            foreach (var progress in progressList)
            {
                list.Add(new DetalladoActividades()
                {
                    actividad = progress.IdElementNavigation.IdActivityNavigation.ActivityName,
                    area = progress.IdAreaNavigation.AreaName,
                    elemento = progress.IdElementNavigation.ElementName,
                    subElemento = progress.IdSubElementNavigation != null ? progress.IdSubElementNavigation.SubElementName : "N/A",
                    numeroApartamento = progress.IdApartmentNavigation.ApartmentNumber,
                    estatus = progress.ProgressLogs != null && progress.ProgressLogs.Count != 0 ? progress.ProgressLogs.Last().IdStatusNavigation.StatusName : _status1,
                    total = progress.TotalPieces,
                    avance = progress.ProgressLogs != null && progress.ProgressLogs.Count != 0 ? Int32.Parse(progress.ProgressLogs.Last().Pieces) : 0,
                    IdProgressLog = progress.ProgressLogs != null && progress.ProgressLogs.Count != 0 ? progress.ProgressLogs.Last().IdProgressLog : null,
                    HasObservationsOrBlobs = progress.ProgressLogs != null && progress.ProgressLogs.Count != 0 ? progressLogHasContent(progress.ProgressLogs) : false
                }) ;
            }
            return list;
        }

        private bool progressLogHasContent(ICollection<ProgressLog> progressLogs)
        {
            bool hasContent = false;
            var progressLog = progressLogs.Last();
            if (!string.IsNullOrEmpty(progressLog.Observation))
                hasContent = true;
            else
            {
                //Si no hay observaciones en ese progressLog verifica si tiene Blobs
                progressLogs = progressLogs.OrderByDescending(x => x.IdProgressLog).ToList();
                foreach (var log in progressLogs)
                {
                    var currentBlob = log.IdBlobs.FirstOrDefault();
                    if (currentBlob != null)
                    {
                        hasContent = true;
                        break;
                    }
                }
            }

            return hasContent;
        }

        private List<string>? getURIBlobs(ICollection<ProgressLog>? logs)
        {
            int contador = 0;
            string? currentUri;
            var list = new List<string>();
            if (logs != null && logs.Count != 0)
            {
                logs = logs.OrderByDescending(x => x.IdProgressLog).ToList();
                foreach (var log in logs)
                {
                    var currentBlobs = log.IdBlobs;
                    foreach (var blob in currentBlobs)
                    {
                        currentUri = blob.Uri;
                        if (currentUri != null)
                        {
                            list.Add(currentUri);
                            contador++;
                        }
                        if (contador >= 3)
                            break;
                    }
                    if (contador >= 3)
                        break;
                }
            }
            return list;
        }

        public async Task<byte[]> GetReporteAvance(List<AparmentProgress> aparmentProgress, string subTitle)
        {
            ReporteAvance reporteAvance = new()
            {
                FechaGeneracion = DateTime.Now,
                Apartments = aparmentProgress
            };
            return _reportesFactory.CrearPdf(reporteAvance, subTitle);
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
                    ApartmentProgress = Math.Truncate(100.00 / total * current)
                });
            }
            return list;
        }

        public async Task<List<AparmentProgress>> GetAparments(int? idBuilding, int? idAparment)
        {
            if (idBuilding == null)
                idBuilding = 1;
            IQueryable<ProgressReport> progressReports = _dbContext.ProgressReports;
            IQueryable<ProgressLog> progressLogs = _dbContext.ProgressLogs;
            //IQueryable<ProgressLog> progressLogsMaxDate = _dbContext.ProgressLogs.GroupBy(x => x.IdProgressReport).Select(x => x.);
            IQueryable<Apartment> apartments = _dbContext.Apartments;

            if (idAparment != null)
                progressReports = progressReports.Where(x => x.IdApartment == idAparment);

            progressReports = progressReports.Where(x => x.IdBuilding == idBuilding);

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
                    ApartmentProgress = Math.Truncate(100.00 / total * current)
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
                    Progress = Math.Truncate( 100.00 / total * current)
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

        public async Task<byte[]> GetReporteAvanceActividad(List<ActivityProgress> activityProgress, string subTitle)
        {
            List<string> activityNames = new List<string>();
            foreach (var activity in activityProgress)
                activityNames.Add(activity.ActivityName);

            var name = listActivities.ElementAt(0).ActivityName;
            ReporteAvanceActividad reporteAvance = new()
            {
                FechaGeneracion = DateTime.Now,
                Activities = activityProgress
            };
            return _reportesFactory.CrearPdf(reporteAvance, subTitle);
        }

        public int? getIdActividadByElement(int idElement)
        {
            var localElement = listElements.FirstOrDefault(x => x.IdElement == idElement);
            if (localElement == null)
                return null;
            var nombreActividad = listActivities.FirstOrDefault(x => x.IdActivity == localElement.IdActivity);
            return nombreActividad == null ? null : nombreActividad.IdActivity;
        }

        public async Task<List<AparmentProgress>> GetActivitiesByAparment(int? idBuilding, int? idActividad)
        {            
            if (idBuilding == null)
                idBuilding = 1;
            IQueryable<ProgressReport> progress = _dbContext.ProgressReports.Where(x => x.IdBuilding == idBuilding);
            var apartmentsId = progress.Select(x => x.IdApartment).Distinct();
            var activitiesId = progress.Select(x => x.IdElementNavigation.IdActivity).Distinct();

            IQueryable<ProgressReport> progressReports = _dbContext.ProgressReports.Include(x => x.IdApartmentNavigation).Include(x => x.IdElementNavigation).Include(x => x.IdElementNavigation.IdActivityNavigation);
            IQueryable<Activity> Activities = _dbContext.Activities;
            Activities = Activities.Where(x => activitiesId.Contains(x.IdActivity));
            IQueryable<ProgressLog> progressLogs = _dbContext.ProgressLogs;
            IQueryable<Apartment> apartments = _dbContext.Apartments;
            apartments = apartments.Where(x => apartmentsId.Contains(x.IdApartment)).OrderBy(x => x.IdApartment);

            var list = new List<AparmentProgress>();

            if (idActividad != null)
                progressReports = progressReports.Where(x => x.IdElementNavigation.IdActivityNavigation.IdActivity == idActividad);

            progressReports = progressReports.Where(x => x.IdBuilding == idBuilding);

            foreach(var activity in Activities)
            {
                if (idActividad != null && activity.IdActivity != idActividad)
                    continue;
                var progressReportsCurrentActivity = progressReports.Where(x => x.IdElementNavigation.IdActivityNavigation.IdActivity.Equals(activity.IdActivity));

                List<TotalPicesByAparment> totalOfPicesByAparment = progressReportsCurrentActivity.GroupBy(x => x.IdApartment)
                    .Select(x => new TotalPicesByAparment
                    {
                        IdAparment = x.Key,
                        Pices = x.Sum(s => Convert.ToInt32(s.TotalPieces))
                    }).ToList();

                var progressReportsCurrentActivityByAparment = progressReportsCurrentActivity.Join(progressLogs, x => x.IdProgressReport, y => y.IdProgressReport, (report, log) => new { report, log }).GroupBy(x => x.report.IdApartment).ToList();

                foreach (var apartment in apartments)
                {
                    Double total = totalOfPicesByAparment.FirstOrDefault(x => x.IdAparment == apartment.IdApartment).Pices;
                    var progressReportsCurrentActivityCurrentAparment = progressReportsCurrentActivityByAparment.FirstOrDefault(x => x.Key == apartment.IdApartment);
                    Double current;
                    if (progressReportsCurrentActivityCurrentAparment != null)
                        current = progressReportsCurrentActivityCurrentAparment.GroupBy(x => x.log.IdProgressReport).Select(x => x.OrderByDescending(x => x.log.DateCreated).FirstOrDefault()).Sum(x => long.Parse(x.log.Pieces));
                    else
                        current = 0;
                    list.Add(new AparmentProgress()
                    {
                        Activity_ = activity.ActivityName,
                        ApartmentNumber = apartment.ApartmentNumber,
                        ApartmentProgress = Math.Truncate( 100.00 / total * current )
                    });
                }

            }            
            return list;
        }

        public async Task<byte[]> GetReporteAvancDeActividadPorDepartamento(List<AparmentProgress> aparmentProgress, bool all)
        {
            var listGroupedByActivity = aparmentProgress.GroupBy(x => x.Activity_).ToList();
            var list = new List<ReporteActividadPorDepartamento>();

            foreach (var actividad in listGroupedByActivity)
            {
                list.Add(new ReporteActividadPorDepartamento()
                {
                    Actividad = actividad.Key,
                    Apartments = aparmentProgress.Where(x => x.Activity_ == actividad.Key).ToList()
                });
            }
            string subtitulo = all ? "(Todos)" : "(Seleccionados)";
            return _reportesFactory.CrearPdf(list, subtitulo);
        }

        public async Task<List<ActivityProgressByAparment>> GetAparmentsByActivity(int? idBuilding, int? idApartment)
        {
            if (idBuilding == null)
                idBuilding = 1;
            IQueryable<ProgressReport> progress = _dbContext.ProgressReports.Where(x => x.IdBuilding == idBuilding);
            var apartmentsId = progress.Select(x => x.IdApartment).Distinct();
            var activitiesId = progress.Select(x => x.IdElementNavigation.IdActivity).Distinct();

            IQueryable<ProgressReport> progressReports = _dbContext.ProgressReports.Include(x => x.IdApartmentNavigation).Include(x => x.IdElementNavigation).Include(x => x.IdElementNavigation.IdActivityNavigation);
            IQueryable<Activity> Activities = _dbContext.Activities;
            Activities = Activities.Where(x => activitiesId.Contains(x.IdActivity));
            IQueryable<Apartment> apartments = _dbContext.Apartments;
            apartments = apartments.Where(x => apartmentsId.Contains(x.IdApartment)).OrderBy(x => x.IdApartment);
            IQueryable<ProgressLog> progressLogs = _dbContext.ProgressLogs;
            var list = new List<ActivityProgressByAparment>();

            if (idApartment != null)
                progressReports = progressReports.Where(x => x.IdApartment == idApartment);

            progressReports = progressReports.Where(x => x.IdBuilding == idBuilding);

            foreach (var aparment in apartments)
            {
                if (idApartment != null && aparment.IdApartment != idApartment)
                    continue;
                var progressReportsCurrentAparment = progressReports.Where(x => x.IdApartmentNavigation.IdApartment.Equals(aparment.IdApartment));

                List<TotalPiecesByActivity> totalPiecesByActivity = progressReportsCurrentAparment.GroupBy(x => x.IdElementNavigation.IdActivity)
                    .Select(x => new TotalPiecesByActivity
                    {
                        IdActivity = x.Key,
                        Pieces = x.Sum(s => Convert.ToInt32(s.TotalPieces))
                    }).ToList();

                var progressReportsCurrentAparmentByActivity = progressReportsCurrentAparment.Join(progressLogs, x => x.IdProgressReport, y => y.IdProgressReport, (report, log) => new { report, log }).GroupBy(x => x.report.IdElementNavigation.IdActivity).ToList();

                foreach (var activity in Activities)
                {
                    Double total = totalPiecesByActivity.FirstOrDefault(x => x.IdActivity == activity.IdActivity).Pieces;
                    Double current;
                    var progressReportsCurrentAparmentByActivityCurrentActivity = progressReportsCurrentAparmentByActivity.FirstOrDefault(x => x.Key == activity.IdActivity);
                    if (progressReportsCurrentAparmentByActivityCurrentActivity != null)
                        current = progressReportsCurrentAparmentByActivityCurrentActivity.GroupBy(x => x.log.IdProgressReport).Select(x => x.OrderByDescending(x => x.log.DateCreated).FirstOrDefault()).Sum(x => long.Parse(x.log.Pieces));
                    else
                        current = 0;
                    list.Add(new ActivityProgressByAparment()
                    {
                        ApartmentNumber = aparment.ApartmentNumber,
                        Activity_ = activity.ActivityName,
                        ApartmentProgress = Math.Truncate(100.00 / total * current)
                    });
                }

            }
            //var listasd = list.GroupBy(x => x.ApartmentNumber);
            return list;
        }

        public async Task<byte[]> GetReporteAvanceDeDepartamentoPorActividad(List<ActivityProgressByAparment> activityProgressByAparment, bool all)
        {
            var listGroupedByActivity = activityProgressByAparment.GroupBy(x => x.ApartmentNumber).ToList();
            var list = new List<ReporteDepartamentoPorActividad>();

            foreach (var aparment in listGroupedByActivity)
            {
                list.Add(new ReporteDepartamentoPorActividad()
                {
                    Aparment = aparment.Key,
                    Activitiess = activityProgressByAparment.Where(x => x.ApartmentNumber == aparment.Key).ToList()
                });
            }
            string subtitulo = all ? "(Todos)" : "(Seleccionados)";
            return _reportesFactory.CrearPdf(list, subtitulo);
        }

        //private Tuple<int, bool>? getIdLogAndContent(int idProgressReportLocal)
        //{
        //    bool hasContent = false;
        //    var progressLog = listProgressLog.LastOrDefault(x => x.IdProgressReport == idProgressReportLocal);
        //    if (progressLog == null)
        //        return null;
        //    if (!string.IsNullOrEmpty(progressLog.Observation))
        //        hasContent = true;
        //    else {
        //        //Si no hay observaciones en ese progressLog verifica si tiene Blobs
        //        var logs = listProgressLog.Where(x => x.IdProgressReport == idProgressReportLocal).ToList();
        //        logs = logs.OrderByDescending(x => x.IdProgressLog).ToList();
        //        foreach (var log in logs)
        //        {
        //            var currentBlob = log.IdBlobs.FirstOrDefault();
        //            if (currentBlob != null)
        //            {
        //                hasContent = true;
        //                break;
        //            }
        //        }
        //    }

        //    return Tuple.Create(progressLog.IdProgressLog, hasContent);
        //} 
    }
}