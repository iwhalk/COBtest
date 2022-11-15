﻿using FrontEnd.Interfaces;
using FrontEnd.Stores;
using Shared.Models;
using SharedLibrary.Models;

namespace FrontEnd.Services
{
    public class ReceptionCertificateService : IReceptionCertificateService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;

        public ReceptionCertificateService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<ActasRecepcion>> GetReceptionCertificatesAsync(string? startDay = null, string? endDay = null, int? certificateType = null, int? propertyType = null, int? numberOfRooms = null, int? lessor = null, int? tenant = null, string? delegation = null, string? agent = null, int? currentPage = null, int? rowNumber = null)
        { 
            string? certificateTypeS = null;
            string? propertyTypeS = null;
            string? numberOfRoomsS = null;
            string? lessorS = null;
            string? tenantS = null;
            string? currentPageS = null;
            string? rowNumberS = null;

            if (certificateType is not null || certificateType > 0)
            {
                certificateTypeS = certificateType.ToString();
            }
            if (propertyType is not null || propertyType > 0)
            {
                propertyTypeS = propertyType.ToString();
            }
            if (numberOfRooms is not null || numberOfRooms > 0)
            {
                numberOfRoomsS = numberOfRooms.ToString();
            }
            if (lessor is not null || lessor > 0)
            {
                lessorS = lessor.ToString();
            }
            if (tenant is not null || tenant > 0)
            {
                tenantS = tenant.ToString();
            }
            if (currentPage is not null || currentPage > 0)
            {
                currentPageS = currentPage.ToString();
            }
            if (rowNumber is not null || rowNumber > 0)
            {
                rowNumberS = rowNumber.ToString();
            }

            if (_context.ActasRecepcion == null)
            {
                var response = await _repository.GetAsync<List<ActasRecepcion>>($"api/ReceptionCertificates?startDay={startDay}&endDay={endDay}&certificateType={certificateTypeS}&propertyType={propertyTypeS}&numberOfRooms={numberOfRoomsS}&lessor={lessorS}&tenant={tenantS}&delegation={delegation}&agent={agent}&currentPage={currentPageS}&rowNumber={rowNumberS}");

                if (response != null)
                {
                    _context.ActasRecepcion = response;
                    return _context.ActasRecepcion;
                }
            }

            return _context.ActasRecepcion;
        }

        public async Task<ReceptionCertificate> PostReceptionCertificatesAsync(ReceptionCertificate receptionCertificate)
        {
            return await _repository.PostAsync("api/ReceptionCertificates", receptionCertificate);
        }
    }
}
