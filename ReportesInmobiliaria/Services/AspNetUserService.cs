using Microsoft.EntityFrameworkCore;
using ReportesInmobiliaria.Interfaces;
using SharedLibrary.Data;
using SharedLibrary.Models;

namespace ReportesInmobiliaria.Services
{
    public class AspNetUserService : IAspNetUserService
    {
        private readonly InmobiliariaDbContext _dbContext;

        public AspNetUserService(InmobiliariaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AspNetUser>?> GetAspNetUsersAsync()
        {
            return await _dbContext.AspNetUsers.ToListAsync();
        }
    }
}
