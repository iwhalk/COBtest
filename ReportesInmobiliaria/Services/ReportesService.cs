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
        //private readonly InmobiliariaDbContextProcedures _dbContextProcedure;

        public ReportesService(ObraDbContext dbContext, IHttpContextAccessor httpContextAccessor, ReportesFactory reportesFactory)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _reportesFactory = reportesFactory;
        }

        public async Task<byte[]> GetReporteDetalles(int idApartment)
        {
            progressReportsComplete = _dbContext.ProgressReports.ToList();
            ReporteDetalles reporteDetalles = new()
            {
                detalladoActividades = GetSubElementsAsync(idApartment)
            };
            if (reporteDetalles.detalladoActividades.Count == 0)
                return null;
            return _reportesFactory.CrearPdf(reporteDetalles);
        }

        public List<DetalladoActividades> GetSubElementsAsync(int idApartment)
        {
            var list = new List<DetalladoActividades>();
            var progressReport = progressReportsComplete.Where(x => x.IdApartment == idApartment).ToList();            
            listElements = _dbContext.Elements.ToList();
            listActivities = _dbContext.Activities.ToList();
            listSubElements= _dbContext.SubElements.ToList();
            foreach (var subElement in progressReport)
            {
                list.Add(new DetalladoActividades()
                {
                    actividad = getActividad(subElement.IdElement),
                    elemento = getElemento(subElement.IdElement),
                    subElemento = getSubElemento(subElement.IdSubElement),
                    estatus = "",
                    total = subElement.TotalPieces,
                    avance = 0,
                });
            }
            return list;
        }
        public async Task<byte[]> GetReporteAvance()
        {
            ReporteAvance reporteAvance = new()
            {
                Apartments = GetAparmentsAsync()
            };
            return _reportesFactory.CrearPdf(reporteAvance);
        }

        public List<AparmentProgress> GetAparmentsAsync()
        {
            IQueryable<ProgressReport> progressReports = _dbContext.ProgressReports;
            IQueryable<ProgressLog> progressLogs= _dbContext.ProgressLogs;
            IQueryable<Apartment> apartments = _dbContext.Apartments;

            var progressReportsByAparment = progressReports.Join(progressLogs, x => x.IdProgressReport, y => y.IdProgressReport, (report, log) => new {report, log}).GroupBy(x => x.report.IdApartment);

            var list = new List<AparmentProgress>();
            foreach (var aparment in progressReportsByAparment)
            {                
                var total = aparment.Sum(x => long.Parse(x.report.TotalPieces));
                var current = aparment.Sum(x => long.Parse(x.log.Pieces));
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
    }
}
