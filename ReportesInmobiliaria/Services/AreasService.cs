using Microsoft.EntityFrameworkCore;
using ReportesObra.Interfaces;
using SharedLibrary.Data;
using SharedLibrary.Models;

namespace ReportesObra.Services
{
    public class AreasService : IAreasService
    {
        private readonly ObraDbContext _dbContext;

        public AreasService(ObraDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Area>?> GetAreasAsync()
        {
            return await _dbContext.Areas.ToListAsync();
        }

        public async Task<Area?> GetAreaAsync(int id)
        {
            return await _dbContext.Areas.FirstOrDefaultAsync(x => x.IdArea == id);
        }

        public async Task<Area?> CreateAreaAsync(Area area)
        {
            await _dbContext.Areas.AddAsync(area);
            try { await _dbContext.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }
            return area;
        }

        public async Task<bool> UpdateAreaAsync(Area area)
        {
            _dbContext.Entry(area).State = EntityState.Modified;
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

        public async Task<bool> DeleteAreaAsync(int id)
        {
            Area? area = _dbContext.Areas.FirstOrDefault(x => x.IdArea == id);
            if (area == null)
                return false;
            _dbContext.Areas.Remove(area);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
