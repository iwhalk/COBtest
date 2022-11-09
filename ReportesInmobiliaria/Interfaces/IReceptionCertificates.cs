using Shared.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface IReceptionCertificates
    {
        Task<List<ReceptionCertificate?>> GetReceptionCertificatesAsync(Dates dates, string? propertyType, string? numberOfRooms, string? lessor, string? tenant, string? delegation, string? agent);
        Task<ReceptionCertificate?> CreateReceptionCertificateAsync(ReceptionCertificate receptionCertificate);
    }
}