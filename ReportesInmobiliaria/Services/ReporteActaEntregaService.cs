using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ReportesInmobiliaria.Interfaces;
using ReportesInmobiliaria.Utilities;
using SharedLibrary.Data;
using SharedLibrary.Models;
using StoredProcedureEFCore;
using System.Collections;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ReportesInmobiliaria.Services
{
    public class ReporteActaEntregaService : IReporteActaEntregaService
    {
        private readonly InmobiliariaDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ReportesFactory _reportesFactory;
        //private readonly InmobiliariaDbContextProcedures _dbContextProcedure;

        public ReporteActaEntregaService(InmobiliariaDbContext dbContext, IHttpContextAccessor httpContextAccessor, ReportesFactory reportesFactory)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _reportesFactory = reportesFactory;
        }

        public async Task<byte[]> GetActaEntrega(int idReceptionCertificate)
        {
            ReporteActaEntrega reporteActaEntrega = new()
            {
                header = await _dbContext.Procedures.SP_GET_AERI_HEADERAsync(idReceptionCertificate.ToString()),
                areas = await _dbContext.Procedures.SP_GET_AERI_AREASAsync(idReceptionCertificate),
                deliverables = await _dbContext.Procedures.SP_GET_AERI_DELIVERABLESAsync(idReceptionCertificate)
            };
            return _reportesFactory.CrearPdf(reporteActaEntrega);
        }
    }
}
