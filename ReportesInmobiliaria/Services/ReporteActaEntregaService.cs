using Microsoft.EntityFrameworkCore;
using ReportesInmobiliaria.Interfaces;
using ReportesInmobiliaria.Utilities;
using Shared.Data;
using Shared.Models;

namespace ReportesInmobiliaria.Services
{
    public class ReporteActaEntregaService : IReporteActaEntregaService
    {
        private readonly InmobiliariaDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ReportesFactory _reportesFactory;
        private readonly InmobiliariaDbContextProcedures _dbContextProcedure;

        public ReporteActaEntregaService(InmobiliariaDbContext dbContext, IHttpContextAccessor httpContextAccessor, ReportesFactory reportesFactory, InmobiliariaDbContextProcedures dbContextProcedure)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _reportesFactory = reportesFactory;
            _dbContextProcedure = dbContextProcedure;
        }

        public async Task<byte[]> GetActaEntrega(int idProperty, int idTenant, string idContrato)
        {
            var result = _dbContextProcedure.SP_GET_AERIAsync(1);
            Property propertyLocal = _dbContext.Properties.FirstOrDefault(x => x.IdProperty == idProperty);
            Lessor lessor = _dbContext.Lessors.FirstOrDefault(x => x.IdLessor == propertyLocal.IdLessor);
            Tenant tenant = _dbContext.Tenants.FirstOrDefault(x => x.IdTenant == idTenant);
            //ReceptionCertificate contrato = _dbContext.ReceptionCertificates.FirstOrDefault(x => x.IdReceptionCertificate == idContrato);
            ReporteActaEntrega reporteActaEntrega = new()
            {
                generationDate = DateTime.Now,
                property = propertyLocal,
                lessor = $"{lessor.Name} {lessor.LastName}",
                tenant = $"{tenant.Name} {tenant.LastName}",
                inventories = await GetInventoriesAsync(idProperty),
                numeroDeContrato = idContrato
            };
            return _reportesFactory.CrearPdf(reporteActaEntrega);
        }

        async Task<List<InventoryToReports>> GetInventoriesAsync(int id)
        {
            IQueryable<Inventory> inventories = _dbContext.Inventories;

            if (id != null)
                inventories = inventories.Where(x => x.IdProperty == id);

            var list = new List<InventoryToReports>();
            int featureID;
            int serviceID;
            foreach (var inventory in inventories)
            {
                featureID = _dbContext.Descriptions.FirstOrDefault(x => x.IdDescription == inventory.IdDescription).IdFeature ;
                serviceID = _dbContext.Features.FirstOrDefault(x => x.IdFeature == featureID).IdService;
                list.Add(new InventoryToReports()
                {
                    //Nombre = nombreCompleto != " " ? nombreCompleto : "",
                    area = _dbContext.Areas.FirstOrDefault(x => x.IdArea == inventory.IdArea).AreaName,
                    feature = _dbContext.Features.FirstOrDefault(x => x.IdFeature == featureID).FeatureName,
                    service = _dbContext.Services.FirstOrDefault(x => x.IdService == serviceID).ServiceName,
                    description = _dbContext.Descriptions.FirstOrDefault(x => x.IdDescription == inventory.IdDescription).DescriptionName,
                    note = inventory.Note ?? "",
                    observation = inventory.Observation ?? ""
                });
            }
            return list;
        }
    }
}
