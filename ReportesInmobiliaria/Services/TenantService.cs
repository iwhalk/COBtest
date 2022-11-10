using SharedLibrary.Data;
using ReportesInmobiliaria.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;

namespace ReportesInmobiliaria.Services
{
    public class TenantService : ITenantService
    {
        private readonly InmobiliariaDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantService(InmobiliariaDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Tenant?>> GetTenantAsync()
        {
            return await _dbContext.Tenants.ToListAsync();
        }

        public async Task<Tenant?> CreateTenantAsync(Tenant tenant)
        {
            await _dbContext.Tenants.AddAsync(tenant);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return tenant;
        }
    }
}
