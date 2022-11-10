using Microsoft.EntityFrameworkCore;
using ReportesInmobiliaria.Interfaces;
using ReportesInmobiliaria.Utilities;
using SharedLibrary.Data;
using SharedLibrary.Models;
using SharedLibrary.Models;

namespace ReportesInmobiliaria.Services
{
    public class ReporteFeaturesService : IReporteFeaturesService
    {
        private readonly InmobiliariaDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ReportesFactory _reportesFactory;

        public ReporteFeaturesService(InmobiliariaDbContext dbContext, IHttpContextAccessor httpContextAccessor, ReportesFactory reportesFactory)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _reportesFactory = reportesFactory;
        }

        public async Task<byte[]> GetReporteFeatures(int? id)
        {
            var features = await _dbContext.Features.ToListAsync();

            ReporteFeatures reporteFeatures = new()
            {
                generationDate = DateTime.Now,
                featuresNumber = features.Count().ToString() ?? "",
                caracteristicas = await GetFeaturesAsync(id)
            };

            return _reportesFactory.CrearPdf(reporteFeatures);
        }

        async Task<List<Caracteristicas>> GetFeaturesAsync(int? id)
        {
            IQueryable<Feature> features = _dbContext.Features;

            if (id != null)
                features = features.Where(x => x.IdFeature == id);

            var list = new List<Caracteristicas>();

            foreach (var feature in features)
            {
                list.Add(new Caracteristicas()
                {
                    //Nombre = nombreCompleto != " " ? nombreCompleto : "",
                    nombre = feature.FeatureName ?? "",
                    idService = feature.IdService.ToString()
                });
            }
            return list;
        }
    }
}
