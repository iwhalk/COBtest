using SharedLibrary.Data;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;
using System.Diagnostics;
using SharedLibrary.Models;
using NuGet.Packaging.Signing;
using ReportesObra.Interfaces;

namespace ReportesObra.Services
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

        public async Task<List<ActasRecepcion?>> GetReceptionCertificatesAsync(Dates dates, int? certificateType, int? propertyType, int? numberOfRooms, int? lessor, int? tenant, string? delegation, string? agent, bool? completed)
        {
            IQueryable<AspNetUser> aspNetUsers = _dbContext.AspNetUsers;
            IQueryable<ReceptionCertificate> receptionCertificates = _dbContext.ReceptionCertificates
                                                                                .Include(x => x.IdTenantNavigation)
                                                                                .Include(x => x.IdPropertyNavigation)
                                                                                .ThenInclude(x => x.IdPropertyTypeNavigation)
                                                                                .Include(x => x.IdPropertyNavigation)
                                                                                .ThenInclude(x => x.IdLessorNavigation);
            if (dates != null)
                receptionCertificates = receptionCertificates.Where(d => d.CreationDate.Date >= dates.StartDate && d.CreationDate <= dates.EndDate);
            if (completed != null && completed != false)
                receptionCertificates = receptionCertificates.Where(x => !string.IsNullOrEmpty(x.ApprovalPathTenant) && !string.IsNullOrEmpty(x.ApprovalPathTenant));
            else if (completed == false || completed == null)
                receptionCertificates = receptionCertificates.Where(x => string.IsNullOrEmpty(x.ApprovalPathTenant) && string.IsNullOrEmpty(x.ApprovalPathTenant));
            if (propertyType != null)
                receptionCertificates = receptionCertificates.Where(x => x.IdPropertyNavigation.IdPropertyTypeNavigation.IdPropertyType == propertyType);
            if (numberOfRooms != null)
                receptionCertificates = receptionCertificates.Where(x => x.IdPropertyNavigation.NumberOfRooms == numberOfRooms);
            if (lessor != null)
                receptionCertificates = receptionCertificates.Where(x => x.IdPropertyNavigation.IdLessorNavigation.IdLessor == lessor);
            if (tenant != null)
                receptionCertificates = receptionCertificates.Where(x => x.IdTenant == tenant);
            if (delegation != null)
                receptionCertificates = receptionCertificates.Where(x => x.IdPropertyNavigation.Delegation == delegation);
            if (agent != null)
                receptionCertificates = receptionCertificates.Where(x => x.IdAgent == agent);
            if (certificateType != null)
                receptionCertificates = receptionCertificates.Where(x => x.IdTypeRecord == certificateType);

            var list = new List<ActasRecepcion>();

            foreach (var certificate in receptionCertificates)
            {
                list.Add(new ActasRecepcion()
                {
                    IdReceptionCertificate = certificate.IdReceptionCertificate,
                    Fecha = certificate.CreationDate,
                    Acta = certificate.IdTypeRecord == 1 ? "Entrada" : "Salida",
                    Inmueble = certificate.IdPropertyNavigation.IdPropertyTypeNavigation.PropertyTypeName ?? "",
                    Habitaciones = certificate.IdPropertyNavigation.NumberOfRooms,
                    Arrendador = certificate.IdPropertyNavigation.IdLessorNavigation.Name + " " + certificate.IdPropertyNavigation.IdLessorNavigation.LastName,
                    Arrendatario = certificate.IdTenantNavigation.Name + " " + certificate.IdTenantNavigation.LastName,
                    Delegacion = certificate.IdPropertyNavigation.Delegation ?? "",
                    Agente = aspNetUsers.FirstOrDefault(x => x.Id == certificate.IdAgent).Name + " " + aspNetUsers.FirstOrDefault(x => x.Id == certificate.IdAgent).LastName,
                    IdProperty = certificate.IdReceptionCertificate
                });
            }
            return list;
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

        public async Task<ReceptionCertificate?> UpdateReceptionCertificateAsync(ReceptionCertificate receptionCertificate)
        {
            _dbContext.ReceptionCertificates.Update(receptionCertificate);
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

        public async Task<List<ReceptionCertificate?>> GetReceptionCertificatesAsync(int? idReceptionCertificate)
        {
            if (idReceptionCertificate != null)
            {
                var certificates = _dbContext.ReceptionCertificates.Where(x => x.IdReceptionCertificate == idReceptionCertificate);
                if (certificates.Any())
                {
                    return await certificates.ToListAsync();
                }
            }
            return await _dbContext.ReceptionCertificates.ToListAsync();
        }
    }
}