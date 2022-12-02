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
    public class ReporteDetallesService : IReporteDetallesService
    {
        private readonly ObraDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ReportesFactory _reportesFactory;
        //private readonly InmobiliariaDbContextProcedures _dbContextProcedure;

        public ReporteDetallesService(ObraDbContext dbContext, IHttpContextAccessor httpContextAccessor, ReportesFactory reportesFactory)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _reportesFactory = reportesFactory;
        }

        public async Task<byte[]> GetReporteDetalles()
        {
            ReporteDetalles reporteDetalles = new()
            {
                detalladoActividades = GetSubElementsAsync()
            };
            return _reportesFactory.CrearPdf(reporteDetalles);
        }

        public List<DetalladoActividades> GetSubElementsAsync()
        {
            var list = new List<DetalladoActividades>();
            var progressReport = _dbContext.ProgressReports;

            foreach (var subElement in progressReport)
            {
                list.Add(new DetalladoActividades()
                {
                    //id = subElement.IdSubElement,
                    //nombre = subElement.SubElementName.ToString(),
                    //idElemento = subElement.IdElement,
                    //Tipo = subElement.Type.ToString()
                    actividad = "",
                    elemento = "",
                    subElemento = "",
                    estatus = "",
                    total = 1,
                    avance = 0,
                });
            }
            return list;
        }

        public string getActividad(int idElement)
        {
            var listaActividades = _dbContext.Activities;
            //activityName = from d in listaActividades 
            return "";
        }
    }
}
