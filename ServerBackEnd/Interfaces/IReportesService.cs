using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IReportesService
    {
        Task<ApiResponse<byte[]>> GetReporteLessorsAsync(int IdLessor);
        Task<ApiResponse<byte[]>> GEetReporteReceptionCertificateAsync(int IdFeature);
    }
}
