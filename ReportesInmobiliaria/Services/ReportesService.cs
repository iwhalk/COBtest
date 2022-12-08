using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ReportesObra.Interfaces;
using ReportesObra.Utilities;
using SharedLibrary.Data;
using SharedLibrary.Models;
using StoredProcedureEFCore;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

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

        public async Task<byte[]> GetReporteDetalles(int idBuilding, List<int> idApartments, List<int> idActivities, List<int> idElements, List<int>? idSubElements)
        {
            var ByBuilding = progressReportsComplete.Where(x => x.IdBuilding == idBuilding);
            var ByAparments = FiltradoIdApartments(ByBuilding.ToList(), idApartments);
            var ByElement = FiltradoIdElements(ByAparments.ToList(), idElements);
            if (idSubElements != null)
            if (idSubElements.Count() != 0)
            {
                ByElement = FiltradoIdSubElements(ByElement.ToList(), idSubElements);
            }

            ReporteDetalles reporteDetalles = new()
            {
                detalladoActividades = GetSubElementsAsync(ByElement.ToList())
            };
            reporteDetalles.detalladoActividades = FiltradoIdActivities(reporteDetalles.detalladoActividades, idActivities);
            if (reporteDetalles.detalladoActividades.Count == 0)
                return null;
            return _reportesFactory.CrearPdf(reporteDetalles);
        }

        //public async Task<byte[]> GetReporteDetalles(int idBuilding, int idApartment, List<int> actividades, int? idElement, int? idSubElement)
        //{
        //    //Se obtiene la lista de nombres de las actividades
        //    List<string> actividadesStr = new List<string>();
        //    foreach (var actividad in actividades)
        //    {
        //        var result = listActivities.FirstOrDefault(x => x.IdActivity == actividad);
        //        if (result == null)
        //            continue;
        //        actividadesStr.Add(result.ActivityName);
        //    }
        //    ReporteDetalles reporteDetalles = new()
        //    {
        //        detalladoActividades = GetSubElementsAsync(idBuilding, idApartment)
        //    };
        //    //Se obtiene la nueva lista de objetos para el reporte filtrando las correspondientes actividades            
        //    List<DetalladoActividades> subList = new List<DetalladoActividades>();
        //    foreach (string actividad in actividadesStr)
        //    {
        //        var result = reporteDetalles.detalladoActividades.Where(x => x.actividad == actividad);
        //        if (result == null)
        //            continue;
        //        subList.AddRange(result);
        //    }
        //    ReporteDetalles reporteDetallesActividades = new() { detalladoActividades = subList };
        //    if (idElement != null)
        //    {
        //        string currentElement = getElemento(idElement);
        //        if (currentElement == null)
        //            return null;
        //        reporteDetallesActividades.detalladoActividades = reporteDetallesActividades.detalladoActividades.Where(x => x.elemento == currentElement).ToList();
        //    }
        //    if (idSubElement != null)
        //    {
        //        string currentSubElement = getSubElemento(idSubElement);
        //        if (currentSubElement == null)
        //            return null;
        //        reporteDetallesActividades.detalladoActividades = reporteDetallesActividades.detalladoActividades.Where(x => x.subElemento == currentSubElement).ToList();
        //    }
        //    if (reporteDetallesActividades.detalladoActividades.Count == 0)
        //        return null;
        //    string apartmentNumber = listApartments.FirstOrDefault(x => x.IdApartment == idApartment).ApartmentNumber;
        //    return _reportesFactory.CrearPdf(reporteDetallesActividades, apartmentNumber);
        //}

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