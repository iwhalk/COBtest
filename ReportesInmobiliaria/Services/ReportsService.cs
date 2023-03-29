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
        List<ProgressReport> progressReportsComplete;
        List<ProgressLog> listProgressLog;
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
        private static ObjectAccessUser accessUser;

        //private readonly InmobiliariaDbContextProcedures _dbContextProcedure;

        public ReportsService(ObraDbContext dbContext, IHttpContextAccessor httpContextAccessor, ReportesFactory reportesFactory, IProgressReportsService progressReportsService, IProgressLogsService progressLogsService)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _reportesFactory = reportesFactory;
            _progressReportsService = progressReportsService;
            _progressLogsService = progressLogsService;
            progressReportsComplete = _dbContext.ProgressReports.ToList();
            listProgressLog = _progressLogsService.GetProgressLogsAsync(null, null, null, null).Result;
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
        //Task<List<DetalladoDepartamentos>>
        public async Task<List<DetalladoDepartamentos>> GetDataDetallesDepartamento(int idBuilding, List<int>? idApartments, List<int>? idAreas, List<int>? idActivities, List<int>? idElements, List<int>? idSubElements)
        {
            if (idApartments != null && idApartments.Count() != 0)
                _titleDetails = "(Seleccionados)";
            else
                _titleDetails = "(Todos)";

            List<ProgressReport> listReport = new List<ProgressReport>();
            listReport = await _progressReportsService.GetProgressReportsDetailedAsync(idBuilding, idApartments, idAreas, idElements, idSubElements, idActivities);            
            
            //listReport = progressReportsComplete.Where(x => x.IdBuilding == idBuilding).ToList();
            //var idSupervisor = listReport.ElementAtOrDefault(0).IdSupervisor;
            //listProgressLog = listProgressLog.Where(x => x.IdSupervisor == idSupervisor).ToList();
            //if (idApartments != null && idApartments.Count() != 0)
            //{
            //    listReport = FiltradoIdApartments(listReport, idApartments);
            //    _titleDetails = "(Seleccionados)";
            //}
            //if (idAreas != null && idAreas.Count() != 0)
            //    listReport = FiltradoIdAreas(listReport, idAreas);
            //if (idElements != null && idElements.Count() != 0)
            //    listReport = FiltradoIdElements(listReport, idElements);
            //if (idSubElements != null && idSubElements.Count() != 0)
            //    listReport = FiltradoIdSubElements(listReport, idSubElements);
            
            listReport = listReport.OrderBy(x => x.IdArea).ToList();
            var list = new List<DetalladoDepartamentos>();
            list = GetDetalladoDepartamentos(listReport);

            //if (idActivities != null && idActivities.Count() != 0)
            //    list = FiltradoIdActivities(list, idActivities);
            return list.OrderBy(x => x.numeroApartamento).ToList();
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
            List<ProgressReport> listReport = new List<ProgressReport>();
            _titleDetails = "(Todos)";
            listReport = progressReportsComplete.Where(x => x.IdBuilding == idBuilding).ToList();
            var idSupervisor = listReport.ElementAtOrDefault(0).IdSupervisor;
            listProgressLog = listProgressLog.Where(x => x.IdSupervisor == idSupervisor).ToList();
            if (idElements != null && idElements.Count() != 0)
                listReport = FiltradoIdElements(listReport, idElements);
            if (idSubElements != null && idSubElements.Count() != 0)
                listReport = FiltradoIdSubElements(listReport, idSubElements);
            if (idApartments != null && idApartments.Count() != 0)
                listReport = FiltradoIdApartments(listReport, idApartments);
            listReport = listReport.OrderBy(x => x.IdArea).ToList();
            //ReporteDetallesActividad reporteDetalles = new()
            //{
            //    detalladoActividades = GetSubElementsActividadesAsync(listReport)
            //};
            //if (idActivities != null && idActivities.Count() != 0){
            //    reporteDetalles.detalladoActividades = FiltradoIdActivities(reporteDetalles.detalladoActividades, idActivities);
            //    _titleDetails = "(Seleccionadas)";
            //}

            //if (reporteDetalles.detalladoActividades.Count == 0)
            //    return null;
            //return _reportesFactory.CrearPdf(reporteDetalles, _titleDetails);
            List<DetalladoActividades> list = GetSubElementsActividadesAsync(listReport);
            if (idActivities != null && idActivities.Count() != 0)
            {
                list = FiltradoIdActivities(list, idActivities);
                _titleDetails = "(Seleccionados)";
            }
            var orderedByApartment = list.OrderBy(x => x.numeroApartamento);
            var orderedByActivity = orderedByApartment.OrderBy(x => x.actividad);
            return orderedByActivity.ToList();
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
                    IdProgressLog = progress.ProgressLogs != null && progress.ProgressLogs.Count != 0 ? progress.ProgressLogs.Last().IdProgressLog : null, // LogResult == null ? null : LogResult.Item1,
                    HasObservationsOrBlobs = false, //LogResult == null ? false : LogResult.Item2
                    Obserbation = progress.ProgressLogs.Count != 0 ? (progress.ProgressLogs.Last().Observation ?? null) : null
                    // progress.ProgressLogs.Count != 0 && progress.ProgressLogs.Last().Observation != null ? progress.ProgressLogs.Last().Observation : null
                }) ;
            }
            return list;
        }

        public List<DetalladoActividades> GetSubElementsActividadesAsync(List<ProgressReport> progressList)
        {
            int contador = 0;
            var list = new List<DetalladoActividades>();
            foreach (var subElement in progressList)
            {
                //if (getStausName(subElement.IdProgressReport) == "Terminado")
                //{
                //    contador++;
                //    continue;
                //}
                var LogResult = getIdLogAndContent(subElement.IdProgressReport);
                list.Add(new DetalladoActividades()
                {
                    actividad = getActividadByElement(subElement.IdElement),
                    area = getArea(subElement.IdArea),
                    elemento = getElemento(subElement.IdElement),
                    subElemento = getSubElemento(subElement.IdSubElement),
                    numeroApartamento = getApartmentNumber(subElement.IdApartment),
                    estatus = getStausName(subElement.IdProgressReport),
                    total = subElement.TotalPieces,
                    avance = getProgress(subElement.IdProgressReport),
                    IdProgressLog = LogResult == null ? null : LogResult.Item1,
                    HasObservationsOrBlobs = LogResult == null ? false : LogResult.Item2
                }) ;
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

        private string? getArea(int idArea)
        {
            var nameArea = listAreas.FirstOrDefault(x => x.IdArea == idArea);
            if(nameArea == null)
                return null;
            return nameArea.AreaName;
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
            //Se espera que el primer status en la BD sea "No iniciada"
            if (result == null)
                return _status1;
            int? idStatus = result.IdStatus;
            switch (idStatus)
            {
                case 1:
                    return _status1;
                case 2:
                    return _status2;
                case 3:
                    return _status3;
                default:
                    return _status1;
            }
        }

        //public string getStausName(List<ProgressLog>? logs)
        //{
        //    if (logs == null)
        //        return _status1;
        //    int? idStatus = logs.LastOrDefault().IdStatus;
        //    switch (idStatus)
        //    {
        //        case 1:
        //            return _status1;
        //        case 2:
        //            return _status2;
        //        case 3:
        //            return _status3;
        //        default:
        //            return _status1;
        //    }
        //}

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

        //private Tuple<int, List<string>, string>? getIdProgressLog(int idProgressReportLocal)
        //{
        //    int contador = 0;
        //    var progressLog = listProgressLog.LastOrDefault(x => x.IdProgressReport == idProgressReportLocal);
        //    if (progressLog == null)
        //        return null;
        //    var logs = listProgressLog.Where(x => x.IdProgressReport == idProgressReportLocal).ToList();
        //    logs = logs.OrderByDescending(x => x.IdProgressLog).ToList();
        //    List<string> listUris = new List<string>();
        //    foreach (var log in logs)
        //    {
        //        var currentBlob = log.IdBlobs.FirstOrDefault();
        //        string? currentUri = currentBlob == null ? null : currentBlob.Uri;
        //        if (currentUri != null)
        //        {
        //            listUris.Add(currentUri);
        //            contador++;
        //        }
        //        if (contador == 3)
        //            break;
        //    }
        //    return Tuple.Create(progressLog.IdProgressLog, listUris, progressLog.Observation);
        //}

        private int? getIdProgressLog(int idProgressReport)
        {
            var progressLog = listProgressLog.LastOrDefault(x => x.IdProgressReport == idProgressReport);
            if (progressLog == null)
                return null;
            return progressLog.IdProgressLog;
        }

        private Tuple<int, bool>? getIdLogAndContent(int idProgressReportLocal)
        {
            bool hasContent = false;
            var progressLog = listProgressLog.LastOrDefault(x => x.IdProgressReport == idProgressReportLocal);
            if (progressLog == null)
                return null;
            if (!string.IsNullOrEmpty(progressLog.Observation))
                hasContent = true;
            else {
                //Si no hay observaciones en ese progressLog verifica si tiene Blobs
                var logs = listProgressLog.Where(x => x.IdProgressReport == idProgressReportLocal).ToList();
                logs = logs.OrderByDescending(x => x.IdProgressLog).ToList();
                foreach (var log in logs)
                {
                    var currentBlob = log.IdBlobs.FirstOrDefault();
                    if (currentBlob != null)
                    {
                        hasContent = true;
                        break;
                    }
                }
            }

            return Tuple.Create(progressLog.IdProgressLog, hasContent);
        }

        //private Tuple<List<string>, string> GetLogContent(int idProgressReport)
        //{
        //    var progressLog = listProgressLog.LastOrDefault(x => x.IdProgressReport == idProgressReport);
        //}

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

        private List<ProgressReport> FiltradoIdAreas(List<ProgressReport> ListAllAreas, List<int> idAreas)
        {
            List<ProgressReport> areasFiltred = new List<ProgressReport>();
            foreach (var idArea in idAreas)
            {
                var subListArea = ListAllAreas.Where(x => x.IdArea == idArea);
                if (subListArea == null)
                    continue;
                areasFiltred.AddRange(subListArea.ToList());
            }
            return areasFiltred;
        }

        private List<DetalladoDepartamentos> FiltradoIdActivities(List<DetalladoDepartamentos> ListAllActivities, List<int> idActivities)
        {
            List<string> activityNames = new List<string>();
            List<DetalladoDepartamentos> activitiesFiltred = new List<DetalladoDepartamentos>();
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
            var listSubElementsNull = ListAllSubElements.Where(x => x.IdSubElement == null).ToList();
            subElementsFiltred.AddRange(listSubElementsNull);
            return subElementsFiltred;
        } 
    }
}