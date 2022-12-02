using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReportesObra.Interfaces;
using SharedLibrary.Data;
using SharedLibrary.Models;

namespace ReportesObra.Services
{
    public class ApartmentsService : IApartmentsService
    {
        private readonly ObraDbContext _dbContext;

        public ApartmentsService(ObraDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Apartment>?> GetApartmentsAsync()
        {
            return await _dbContext.Apartments.ToListAsync();
        }

        public async Task<Apartment?> GetApartmentAsync(int id)
        {
            return await _dbContext.Apartments.FirstOrDefaultAsync(x => x.IdApartment == id);
        }

        public async Task<Apartment?> CreateApartmentAsync(Apartment apartment)
        {
            await _dbContext.Apartments.AddAsync(apartment);
            try { await _dbContext.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }
            return apartment;
        }

        public async Task<bool> UpdateApartmentAsync(Apartment apartment)
        {
            _dbContext.Entry(apartment).State = EntityState.Modified;
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

        public async Task<bool> DeleteApartmentAsync(int id)
        {
            Apartment? apartment = _dbContext.Apartments.FirstOrDefault(x => x.IdApartment == id);
            if (apartment == null)
                return false;
            _dbContext.Apartments.Remove(apartment);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
