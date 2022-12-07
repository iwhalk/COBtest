using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
            progressReportsComplete = _dbContext.ProgressReports.ToList();
            listProgressLog = _dbContext.ProgressLogs.ToList();
            listElements = _dbContext.Elements.ToList();
            listActivities = _dbContext.Activities.ToList();
            listSubElements = _dbContext.SubElements.ToList();
            listApartments = _dbContext.Apartments.ToList();
        }

        public async Task<byte[]> GetReporteDetalles(int idBuilding, int idApartment)
        {            
            ReporteDetalles reporteDetalles = new()
            {
                detalladoActividades = GetSubElementsAsync(idBuilding, idApartment)
            };
            if (reporteDetalles.detalladoActividades.Count == 0)
                return null;
            string apartmentNumber = listApartments.FirstOrDefault(x => x.IdApartment == idApartment).ApartmentNumber;
            return _reportesFactory.CrearPdf(reporteDetalles, apartmentNumber);
        }

        public async Task<byte[]> GetReporteDetalles(int idBuilding, int idApartment, List<int> actividades)
        {
            //Se obtiene la lista de nombres de las actividades
            List<string> actividadesStr = new List<string>();
            foreach (var actividad in actividades) 
            {
                var result = listActivities.FirstOrDefault(x => x.IdActivity == actividad);
                if (result == null)
                    continue;
                actividadesStr.Add(result.ActivityName);
            }
            ReporteDetalles reporteDetalles = new()
            {
                detalladoActividades = GetSubElementsAsync(idBuilding, idApartment)
            };
            //Se obtiene la nueva lista de objetos para el reporte filtrando las correspondientes actividades            
            List<DetalladoActividades> subList = new List<DetalladoActividades>();
            foreach (string actividad in actividadesStr)
            {
                var result = reporteDetalles.detalladoActividades.Where(x => x.actividad == actividad);
                if(result == null)
                    continue;
                subList.AddRange(result);
            }
            ReporteDetalles reporteDetallesActividades = new() { detalladoActividades = subList };
            if (reporteDetallesActividades.detalladoActividades.Count == 0)
                return null;
            string apartmentNumber = listApartments.FirstOrDefault(x => x.IdApartment == idApartment).ApartmentNumber;
            return _reportesFactory.CrearPdf(reporteDetallesActividades, apartmentNumber);
        }

        public List<DetalladoActividades> GetSubElementsAsync(int idBuilding, int idApartment)
        {
            int contador = 0;
            var list = new List<DetalladoActividades>();
            var progressReport = progressReportsComplete.Where(x => (x.IdBuilding == idBuilding && x.IdApartment == idApartment)).ToList();                        
            foreach (var subElement in progressReport)
            {
                if (getStausName(subElement.IdProgressReport) == "Terminado")
                {
                    contador++;
                    continue;
                }
                    
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