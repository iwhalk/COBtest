using SharedLibrary.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface IReceptionCertificates
    {
        Task<List<ActasRecepcion?>> GetReceptionCertificatesAsync(Dates dates,int? certificateType, int? propertyType, int? numberOfRooms, int? lessor, int? tenant, string? delegation, string? agent, bool? completed);
        Task<ReceptionCertificate?> CreateReceptionCertificateAsync(ReceptionCertificate receptionCertificate);
        Task<ReceptionCertificate?> UpdateReceptionCertificateAsync(ReceptionCertificate receptionCertificate);
    }
}