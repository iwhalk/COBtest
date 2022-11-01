using Shared.Data;
using ReportesInmobiliaria.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace ReportesInmobiliaria.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly InmobiliariaDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InventoryService(InmobiliariaDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Inventory?>> GetInventoryAsync()
        {
            return await _dbContext.Inventories.ToListAsync();
        }

        public async Task<Inventory?> CreateInventoryAsync(Inventory inventory)
        {
            await _dbContext.Inventories.AddAsync(inventory);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return inventory;
        }
    }
}