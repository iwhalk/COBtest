using Microsoft.EntityFrameworkCore;
using ReportesInmobiliaria.Interfaces;
using SharedLibrary.Data;
using SharedLibrary.Models;
using SharedLibrary.Models;
using System.DirectoryServices.ActiveDirectory;

namespace ReportesInmobiliaria.Services
{
    public class PropertiesService : IPropertiesService
    {
        private readonly InmobiliariaDbContext _dbContext;

        public PropertiesService(InmobiliariaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Property>?> GetPropertiesAsync()
        {
            return await _dbContext.Properties.ToListAsync();
        }

        public async Task<Property?> GetPropertyAsync(int id)
        {
            return await _dbContext.Properties.FirstOrDefaultAsync(x => x.IdProperty == id);
        }

        public async Task<Property?> GetPropertyAsync(string propertyName)
        {
            return await _dbContext.Properties.FirstOrDefaultAsync(x => x.PropertyName == propertyName);
        }

        public async Task<Property?> CreatePropertyAsync(Property property)
        {
            await _dbContext.Properties.AddAsync(property);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return property;
        }

        public async Task<bool> UpdatePropertyAsync(Property property)
        {
            _dbContext.Entry(property).State = EntityState.Modified;
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

        public async Task<bool> DeletePropertyAsync(int id)
        {
            Property? property = _dbContext.Properties.FirstOrDefault(x => x.IdProperty == id);
            if (property == null)
                return false;
            _dbContext.Properties.Remove(property);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
