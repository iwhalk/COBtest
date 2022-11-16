using SharedLibrary.Data;
using ReportesInmobiliaria.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;

namespace ReportesInmobiliaria.Services
{
    public class DescriptionService : IDescriptionService
    {
        private readonly InmobiliariaDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DescriptionService(InmobiliariaDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Description?>> GetDescriptionAsync()
        {
            return await _dbContext.Descriptions.ToListAsync();
        }

        public async Task<Description?> CreateDescriptionAsync(Description description)
        {
            await _dbContext.Descriptions.AddAsync(description);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return description;
        }
    }
}
