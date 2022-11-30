using Microsoft.EntityFrameworkCore;
using ReportesObra.Interfaces;
using SharedLibrary.Data;
using SharedLibrary.Models;

namespace ReportesObra.Services
{
    public class BuildingsService : IBuildingsService
    {
        private readonly ObraDbContext _dbContext;

        public BuildingsService(ObraDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Building>?> GetBuildingsAsync()
        {
            return await _dbContext.Buildings.ToListAsync();
        }

        public async Task<Building?> GetBuildingAsync(int id)
        {
            return await _dbContext.Buildings.FirstOrDefaultAsync(x => x.IdBuilding == id);
        }

        public async Task<Building?> CreateBuildingAsync(Building building)
        {
            await _dbContext.Buildings.AddAsync(building);
            try { await _dbContext.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }
            return building;
        }

        public async Task<bool> UpdateBuildingAsync(Building building)
        {
            _dbContext.Entry(building).State = EntityState.Modified;
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

        public async Task<bool> DeleteBuildingAsync(int id)
        {
            Building? building = _dbContext.Buildings.FirstOrDefault(x => x.IdBuilding == id);
            if (building == null)
                return false;
            _dbContext.Buildings.Remove(building);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
