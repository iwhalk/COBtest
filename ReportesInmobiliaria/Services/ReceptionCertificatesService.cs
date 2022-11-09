using Shared.Data;
using ReportesInmobiliaria.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace ReportesInmobiliaria.Services
{
    public class ReceptionCertificatesService : IReceptionCertificates
    {
        private readonly InmobiliariaDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReceptionCertificatesService(InmobiliariaDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ReceptionCertificate?>> GetReceptionCertificatesAsync(Dates dates, string? propertyType, string? numberOfRooms, string? lessor, string? tenant, string? delegation, string? agent)
        {
            IQueryable<AspNetUser> aspNetUsers = _dbContext.AspNetUsers;
            IQueryable<ReceptionCertificate> receptionCertificates = _dbContext.ReceptionCertificates
                                                                                .Include(x => x.IdTenantNavigation)
                                                                                .Include(x => x.IdPropertyNavigation);

            if (propertyType != null)
                receptionCertificates = receptionCertificates.Where(x => x.IdPropertyNavigation.IdPropertyTypeNavigation.PropertyTypeName == propertyType);

            if(dates != null)
                receptionCertificates = receptionCertificates.Where(d => d.CreationDate.Date >= dates.StartDate && d.CreationDate <= dates.EndDate);

            return await receptionCertificates.ToListAsync();
        }

        public async Task<ReceptionCertificate?> CreateReceptionCertificateAsync(ReceptionCertificate receptionCertificate)
        {
            await _dbContext.ReceptionCertificates.AddAsync(receptionCertificate);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return receptionCertificate;
        }
    }
}