using Microsoft.EntityFrameworkCore;
using ReportesObra.Interfaces;
using SharedLibrary.Data;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;
//using System.DirectoryServices.ActiveDirectory;
using System.Security.Claims;

namespace ReportesObra.Services
{
    public class LessorService : ILessorService
    {
        private readonly InmobiliariaDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LessorService(InmobiliariaDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Lessor>> GetLessorsAsync()
        {
            return await _dbContext.Lessors.ToListAsync();
        }

        public async Task<Lessor?> CreateLessorAsync(Lessor lessor)
        {
            await _dbContext.Lessors.AddAsync(lessor);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return lessor;
        }
    }
}
