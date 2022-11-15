using SharedLibrary.Models;
using Shared.Models;

namespace FrontEnd.Interfaces
{
    public interface IReceptionCertificateService
    {
        Task<List<ActasRecepcion>> GetReceptionCertificatesAsync(string? startDay, string? endDay, int? certificateType, int? propertyType, int? numberOfRooms, int? lessor, int? tenant, string? delegation, string? agent, int? currentPage, int? rowNumber);
        Task<ReceptionCertificate> PostReceptionCertificatesAsync(ReceptionCertificate receptionCertificate);
    }
}
