using Microsoft.EntityFrameworkCore;
using ReportesObra.Interfaces;
using SharedLibrary.Data;
using SharedLibrary.Models;

namespace ReportesObra.Services
{
    public class PropertyTypesService : IPropertyTypesService
    {
        private readonly InmobiliariaDbContext _dbContext;

        public PropertyTypesService(InmobiliariaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<PropertyType>?> GetPropertyTypesAsync()
        {
            return await _dbContext.PropertyTypes.ToListAsync();
        }

        public async Task<PropertyType?> GetPropertyTypeAsync(int id)
        {
            return await _dbContext.PropertyTypes.FirstOrDefaultAsync(x => x.IdPropertyType == id);
        }

        public async Task<PropertyType?> GetPropertyTypeAsync(string name)
        {
            return await _dbContext.PropertyTypes.FirstOrDefaultAsync(x => x.PropertyTypeName == name);
        }

        public async Task<PropertyType?> CreatePropertyTypeAsync(PropertyType propertyType)
        {
            await _dbContext.PropertyTypes.AddAsync(propertyType);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return propertyType;
        }

        public async Task<bool> UpdatePropertyTypeAsync(PropertyType propertyType)
        {
            _dbContext.Entry(propertyType).State = EntityState.Modified;
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

        public async Task<bool> DeletePropertyTypeAsync(string name)
        {
            PropertyType? propertyType = _dbContext.PropertyTypes.FirstOrDefault(x => x.PropertyTypeName == name);
            if (propertyType == null)
                return false;
            _dbContext.PropertyTypes.Remove(propertyType);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
