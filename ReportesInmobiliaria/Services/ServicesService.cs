using SharedLibrary.Data;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;
using ReportesObra.Interfaces;

namespace ReportesObra.Services
{
    public class ServicesService : IServicesService
    {
        private readonly InmobiliariaDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServicesService(InmobiliariaDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Service?>> GetServicesAsync()
        {
            return await _dbContext.Services.ToListAsync();
        }

        public async Task<Service?> CreateServicesAsync(Service service)
        {
            await _dbContext.Services.AddAsync(service);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return service;
        }
    }
}