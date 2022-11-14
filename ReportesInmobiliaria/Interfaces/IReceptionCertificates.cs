using SharedLibrary.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface IReceptionCertificates
    {
        Task<List<ActasRecepcion?>> GetReceptionCertificatesAsync(Dates dates,int? certificateType, int? propertyType, int? numberOfRooms, int? lessor, int? tenant, string? delegation, string? agent);
        Task<ReceptionCertificate?> CreateReceptionCertificateAsync(ReceptionCertificate receptionCertificate);
    }
}