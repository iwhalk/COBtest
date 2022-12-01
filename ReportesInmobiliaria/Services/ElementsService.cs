using Microsoft.EntityFrameworkCore;
using ReportesObra.Interfaces;
using SharedLibrary.Data;
using SharedLibrary.Models;

namespace ReportesObra.Services
{
    public class ElementsService : IElementsService
    {
        private readonly ObraDbContext _dbContext;

        public ElementsService(ObraDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Element>?> GetElementsAsync()
        {
            return await _dbContext.Elements.ToListAsync();
        }

        public async Task<Element?> GetElementAsync(int id)
        {
            return await _dbContext.Elements.FirstOrDefaultAsync(x => x.IdElement == id);
        }

        public async Task<Element?> CreateElementAsync(Element element)
        {
            await _dbContext.Elements.AddAsync(element);
            try { await _dbContext.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }
            return element;
        }

        public async Task<bool> UpdateElementAsync(Element element)
        {
            _dbContext.Entry(element).State = EntityState.Modified;
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

        public async Task<bool> DeleteElementAsync(int id)
        {
            Element? element = _dbContext.Elements.FirstOrDefault(x => x.IdElement == id);
            if (element == null)
                return false;
            _dbContext.Elements.Remove(element);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
