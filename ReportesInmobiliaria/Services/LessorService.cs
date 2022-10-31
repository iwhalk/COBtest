using Microsoft.EntityFrameworkCore;
using ReportesInmobiliaria.Interfaces;
using Shared.Data;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ReportesInmobiliaria.Services
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

        public async Task<List<Lessor>> GetLessors()
        {
            return await _dbContext.Lessors.ToListAsync();
        }
    }


}
