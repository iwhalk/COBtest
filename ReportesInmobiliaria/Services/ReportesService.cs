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
        //private readonly InmobiliariaDbContextProcedures _dbContextProcedure;

        public ReportesService(ObraDbContext dbContext, IHttpContextAccessor httpContextAccessor, ReportesFactory reportesFactory)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _reportesFactory = reportesFactory;
        }

        public async Task<byte[]> GetReporteDetalles()
        {
            ReporteDetalles reporteDetalles = new()
            {
                SubElementos = GetSubElementsAsync()
            };
            return _reportesFactory.CrearPdf(reporteDetalles);
        }

        public List<SubElementosTest> GetSubElementsAsync()
        {
            var list = new List<SubElementosTest>();
            var subElements = _dbContext.SubElements;
            foreach (var subElement in subElements)
            {
                list.Add(new SubElementosTest()
                {
                    id = subElement.IdSubElement,
                    nombre = subElement.SubElementName.ToString(),
                    idElemento = subElement.IdElement,
                    Tipo = subElement.Type.ToString()
                });
            }
            return list;
        }
    }
}
