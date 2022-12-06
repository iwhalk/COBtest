using Microsoft.EntityFrameworkCore;
using ReportesObra.Interfaces;
using SharedLibrary.Data;
using SharedLibrary.Models;

namespace ReportesObra.Services
{
    public class SubElementsService : ISubElementsService
    {
        private readonly ObraDbContext _dbContext;

        public SubElementsService(ObraDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SubElement>?> GetSubElementsAsync(int? ID_Element)
        {
            IQueryable<SubElement> subElements = _dbContext.SubElements;

            if (ID_Element != null)
            {
                subElements = subElements.Where(x => x.IdElement == ID_Element);
            }

            return await subElements.ToListAsync();
        }

        public async Task<SubElement?> GetSubElementAsync(int id)
        {
            return await _dbContext.SubElements.FirstOrDefaultAsync(x => x.IdSubElement == id);
        }

        public async Task<SubElement?> CreateSubElementAsync(SubElement subElement)
        {
            await _dbContext.SubElements.AddAsync(subElement);
            try { await _dbContext.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }
            return subElement;
        }

        public async Task<bool> UpdateSubElementAsync(SubElement subElement)
        {
            _dbContext.Entry(subElement).State = EntityState.Modified;
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

        public async Task<bool> DeleteSubElementAsync(int id)
        {
            SubElement? subElement = _dbContext.SubElements.FirstOrDefault(x => x.IdSubElement == id);
            if (subElement == null)
                return false;
            _dbContext.SubElements.Remove(subElement);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
