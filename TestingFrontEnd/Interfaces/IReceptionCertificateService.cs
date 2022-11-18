using SharedLibrary.Models;

namespace FrontEnd.Interfaces
{
    public interface IReceptionCertificateService
    {
        Task<List<ActasRecepcion>> GetReceptionCertificatesAsync(string? startDay, string? endDay, int? certificateType, int? propertyType, int? numberOfRooms, int? lessor, int? tenant, string? delegation, string? agent, int? currentPage, int? rowNumber, bool completed);
        Task<ReceptionCertificate> PostReceptionCertificatesAsync(ReceptionCertificate receptionCertificate);
        Task<ReceptionCertificate> PutReceptionCertificatesAsync(ReceptionCertificate receptionCertificate);
    }
}
