using SharedLibrary.Models;

namespace FrontEnd.Interfaces
{
    public interface IReceptionCertificateService
    {
        Task<List<ActasRecepcion>> GetReceptionCertificatesAsync(string? day, string? week, string? month, string? propertyType, string? numberOfRooms, string? lessor, string? tenant, string? delegation, string? agent, string? currentPage, string? rowNumber);
        Task<ReceptionCertificate> PostReceptionCertificatesAsync(ReceptionCertificate receptionCertificate);
    }
}
