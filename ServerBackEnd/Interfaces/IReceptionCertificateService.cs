﻿using SharedLibrary.Models;
using SharedLibrary;
using SharedLibrary.Models;

namespace ApiGateway.Interfaces
{
    public interface IReceptionCertificateService
    {
        Task<ApiResponse<List<ActasRecepcion>>> GetReceptionCertificateAsync(string? startDay, string? endDay, int? certificateType, int? propertyType, int? numberOfRooms, int? lessor, int? tenant, string? delegation, string? agent, int? currentPage, int? rowNumber, bool completed);
        Task<ApiResponse<List<ReceptionCertificate>>> GetReceptionCertificateAsync(int? idCertificate);
        Task<ApiResponse<ReceptionCertificate>> PostReceptionCertificateAsync(ReceptionCertificate reception);
        Task<ApiResponse<ReceptionCertificate>> PutReceptionCertificateAsync(ReceptionCertificate reception);
    }
}
