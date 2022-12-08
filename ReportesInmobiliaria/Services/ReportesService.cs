using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using ReportesObra.Interfaces;
using ReportesObra.Utilities;
using SharedLibrary.Data;
using SharedLibrary.Models;
using StoredProcedureEFCore;
using System.Collections;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ReportesObra.Services
{
    public class ReportesService : IReportesService
    {
        private readonly ObraDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ReportesFactory _reportesFactory;
        List<ProgressReport> progressReportsComplete;
        List<ProgressLog> listProgressLog;
        List<Element> listElements;
        List<Activity> listActivities;
        List<SubElement> listSubElements;
        List<Apartment> listApartments;
        //private readonly InmobiliariaDbContextProcedures _dbContextProcedure;

        public ReportesService(ObraDbContext dbContext, IHttpContextAccessor httpContextAccessor, ReportesFactory reportesFactory)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _reportesFactory = reportesFactory;
        }

        public async Task<byte[]> GetReporteDetalles(int idBuilding, int idApartment)
        {
            progressReportsComplete = _dbContext.ProgressReports.ToList();
            listProgressLog = _dbContext.ProgressLogs.ToList();
            ReporteDetalles reporteDetalles = new()
            {
                detalladoActividades = GetSubElementsAsync(idBuilding, idApartment)
            };
            if (reporteDetalles.detalladoActividades.Count == 0)
                return null;
            string apartmentNumber = listApartments.FirstOrDefault(x => x.IdApartment == idApartment).ApartmentNumber;
            return _reportesFactory.CrearPdf(reporteDetalles, apartmentNumber);
        }

        public List<DetalladoActividades> GetSubElementsAsync(int idBuilding, int idApartment)
        {
            var list = new List<DetalladoActividades>();
            var progressReport = progressReportsComplete.Where(x => (x.IdBuilding == idBuilding && x.IdApartment == idApartment)).ToList();            
            listElements = _dbContext.Elements.ToList();
            listActivities = _dbContext.Activities.ToList();
            listSubElements = _dbContext.SubElements.ToList();
            listApartments = _dbContext.Apartments.ToList();
            foreach (var subElement in progressReport)
            {
                if (getStausName(subElement.IdProgressReport) == "Terminado")
                    continue;
                list.Add(new DetalladoActividades()
                {
                    actividad = getActividad(subElement.IdElement),
                    elemento = getElemento(subElement.IdElement),
                    subElemento = getSubElemento(subElement.IdSubElement),
                    estatus = getStausName(subElement.IdProgressReport),
                    total = subElement.TotalPieces,
                    avance = getProgress(subElement.IdProgressReport),
                });
            }
            return list;
        }
        public async Task<byte[]> GetReporteAvance(int? idAparment)
        {
            ReporteAvance reporteAvance = new()
            {
                FechaGeneracion = DateTime.Now,
                Apartments = GetAparmentsAsync(idAparment)
            };
            return _reportesFactory.CrearPdf(reporteAvance, null);
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
            //IQueryable<ProgressLog> progressLogsMaxDate = _dbContext.ProgressLogs.GroupBy(x => x.IdProgressReport).Select(x => x.);
            IQueryable<Apartment> apartments = _dbContext.Apartments;

            if (idAparment != null)
                progressReports = progressReports.Where(x => x.IdApartment == idAparment);

            var progressReportsByAparment = progressReports.Join(progressLogs, x => x.IdProgressReport, y => y.IdProgressReport, (report, log) => new {report, log}).GroupBy(x => x.report.IdApartment);

            var list = new List<AparmentProgress>();
            foreach (var aparment in progressReportsByAparment)
            {                
                var total = aparment.Sum(x => long.Parse(x.report.TotalPieces));
                var current = aparment.GroupBy(x => x.log.IdProgressReport).Select(x => x.OrderByDescending(x => x.log.DateCreated).FirstOrDefault()).Sum(x => long.Parse(x.log.Pieces));
                list.Add(new AparmentProgress()
                {
                    ApartmentNumber = apartments.FirstOrDefault(x => x.IdApartment == aparment.Key).ApartmentNumber,
                    ApartmentProgress = (100 / Convert.ToSingle(total)) * current
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

            var progressReportsByAparment = progressReports.Join(progressLogs, x => x.IdProgressReport, y => y.IdProgressReport, (report, log) => new { report, log }).GroupBy(x => x.report.IdApartment);

            var list = new List<AparmentProgress>();
            foreach (var aparment in progressReportsByAparment)
            {
                var total = aparment.Sum(x => long.Parse(x.report.TotalPieces));
                var current = aparment.GroupBy(x => x.log.IdProgressReport).Select(x => x.OrderByDescending(x => x.log.DateCreated).FirstOrDefault()).Sum(x => long.Parse(x.log.Pieces));
                list.Add(new AparmentProgress()
                {
                    ApartmentNumber = apartments.FirstOrDefault(x => x.IdApartment == aparment.Key).ApartmentNumber,
                    ApartmentProgress = (100 / Convert.ToSingle(total)) * current
                });
            }
            return list;
        }

        public string getActividad(int idElement)
        {
            var nombreActividad = listActivities.FirstOrDefault(x => x.IdActivity == listElements.FirstOrDefault(y => y.IdElement == idElement).IdActivity).ActivityName;
            return nombreActividad;
        }

        public string getElemento(int idElement)
        {
            var nameElement = listElements.FirstOrDefault(x => x.IdElement == idElement).ElementName;
            return nameElement;
        }

        public string getSubElemento(int? idSubElement)
        {
            if (idSubElement == null)
                return "N/A";
            var nameSubElement = listSubElements.FirstOrDefault(x => x.IdSubElement == idSubElement).SubElementName;            
            return nameSubElement;
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
    }
}