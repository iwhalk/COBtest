using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ReportesInmobiliaria.Interfaces;
using ReportesInmobiliaria.Utilities;
using Shared.Data;
using Shared.Models;
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

        public async Task<byte[]> GetActaEntrega(int idProperty)
        {
            ReporteActaEntrega reporteActaEntrega = new()
            {
                header = await _dbContext.Procedures.SP_GET_AERI_HEADERAsync(idProperty),
                areas = await _dbContext.Procedures.SP_GET_AERI_AREASAsync(idProperty),
                deliverables = await _dbContext.Procedures.SP_GET_AERI_DELIVERABLESAsync(idProperty),
            };
            return _reportesFactory.CrearPdf(reporteActaEntrega);
        }

        //async Task<List<InventoryToReports>> GetInventoriesAsync(int id)
        //{
        //    IQueryable<Inventory> inventories = _dbContext.Inventories;

        //    if (id != null)
        //        inventories = inventories.Where(x => x.IdProperty == id);

        //    var list = new List<InventoryToReports>();
        //    int featureID;
        //    int serviceID;
        //    foreach (var inventory in inventories)
        //    {
        //        featureID = _dbContext.Descriptions.FirstOrDefault(x => x.IdDescription == inventory.IdDescription).IdFeature ;
        //        serviceID = _dbContext.Features.FirstOrDefault(x => x.IdFeature == featureID).IdService;
        //        list.Add(new InventoryToReports()
        //        {
        //            //Nombre = nombreCompleto != " " ? nombreCompleto : "",
        //            area = _dbContext.Areas.FirstOrDefault(x => x.IdArea == inventory.IdArea).AreaName,
        //            feature = _dbContext.Features.FirstOrDefault(x => x.IdFeature == featureID).FeatureName,
        //            service = _dbContext.Services.FirstOrDefault(x => x.IdService == serviceID).ServiceName,
        //            description = _dbContext.Descriptions.FirstOrDefault(x => x.IdDescription == inventory.IdDescription).DescriptionName,
        //            note = inventory.Note ?? "",
        //            observation = inventory.Observation ?? ""
        //        });
        //    }
        //    return list;
        //}
    }
}
