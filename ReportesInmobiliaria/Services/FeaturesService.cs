using Microsoft.EntityFrameworkCore;
using ReportesInmobiliaria.Interfaces;
using Shared.Data;
using Shared.Models;

namespace ReportesInmobiliaria.Services
{
    public class FeaturesService : IFeaturesService
    {
        private readonly InmobiliariaDbContext _dbContext;

        public FeaturesService(InmobiliariaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Feature>?> GetFeaturesAsync()
        {
            return await _dbContext.Features.ToListAsync();
        }

        public async Task<Feature?> CreateFeatureAsync(Feature feature)
        {
            await _dbContext.Features.AddAsync(feature);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return feature;
        }

        public async Task<bool> UpdateFeatureAsync(Feature feature)
        {
            _dbContext.Entry(feature).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return true;
        }

        public async Task<bool> DeleteFeatureAsync(int id)
        {
            Feature? feature = _dbContext.Features.FirstOrDefault(x => x.IdFeature == id);
            if (feature == null)
                return false;
            _dbContext.Features.Remove(feature);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}