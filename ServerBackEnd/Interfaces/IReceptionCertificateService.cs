using Shared.Models;
using Shared;
using SharedLibrary.Models;

namespace ApiGateway.Interfaces
{
    public interface IReceptionCertificateService
    {
        Task<ApiResponse<List<ActasRecepcion>>> GetReceptionCertificateAsync(string? day, string? week, string? month, int? propertyType, int? numberOfRooms, int? lessor, int? tenant, string? delegation, string? agent, int? currentPage, int? rowNumber);
        Task<ApiResponse<ReceptionCertificate>> PostReceptionCertificateAsync(ReceptionCertificate reception);
    }
}
