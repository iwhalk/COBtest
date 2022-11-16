﻿using SharedLibrary.Data;
using ReportesInmobiliaria.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;
using System.Diagnostics;
using SharedLibrary.Models;
using NuGet.Packaging.Signing;

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

        public async Task<List<ActasRecepcion?>> GetReceptionCertificatesAsync(Dates dates, int? certificateType, int? propertyType, int? numberOfRooms, int? lessor, int? tenant, string? delegation, string? agent)
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
                    Fecha = certificate.CreationDate,
                    Acta = certificate.IdTypeRecord == 1 ? "Entrada" : "Salida",
                    Inmueble = certificate.IdPropertyNavigation.IdPropertyTypeNavigation.PropertyTypeName ?? "",
                    Habitaciones = certificate.IdPropertyNavigation.NumberOfRooms,
                    Arrendador = certificate.IdPropertyNavigation.IdLessorNavigation.Name + " " + certificate.IdPropertyNavigation.IdLessorNavigation.LastName,
                    Arrendatario = certificate.IdTenantNavigation.Name + " " + certificate.IdTenantNavigation.Name,
                    Delegacion = certificate.IdPropertyNavigation.Delegation ?? "",
                    Agente = aspNetUsers.FirstOrDefault(x => x.Id == certificate.IdAgent).Name + " " + aspNetUsers.FirstOrDefault(x => x.Id == certificate.IdAgent).LastName
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
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }
            return receptionCertificate;
        }   
    }
}