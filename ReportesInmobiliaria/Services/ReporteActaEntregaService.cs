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

        //public async Task<byte[]> GetActaEntrega(int idReceptionCertificate)
        //{
        //    ReporteActaEntrega reporteActaEntrega = new()
        //    {
        //        header = await _dbContext.Procedures.SP_GET_AERI_HEADERAsync(idReceptionCertificate),
        //        areas = await _dbContext.Procedures.SP_GET_AERI_AREASAsync(idReceptionCertificate.ToString()),
        //        deliverables = await _dbContext.Procedures.SP_GET_AERI_DELIVERABLESAsync(idReceptionCertificate.ToString())
        //    };
        //    return _reportesFactory.CrearPdf(reporteActaEntrega);
        //}
    }
}
