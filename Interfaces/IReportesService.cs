using ApiGateway.Models;
using ReportesData.Models;

namespace ApiGateway.Interfaces
{
    public interface IReportesService
    {
        Task<ApiResponse<UsuarioPlaza>> GetUsuarioPlazaAsync();
        Task<ApiResponse<List<TypeDelegacion>>> GetDelegaciones();
        Task<ApiResponse<List<TypePlaza>>> GetPlazas();
        Task<ApiResponse<KeyValuePair<string, string>>> GetTurnos();
        Task<ApiResponse<List<Personal>>> GetAdministradores();
        Task<ApiResponse<List<Personal>>> GetEncargadosTurno();
        Task<byte[]> CreateReporteCajeroReceptorAsync(CajeroReceptor cajeroReceptor);
        Task<byte[]> CreateReporteTurnoCarrilesAsync(TurnoCarriles turnoCarriles);
        Task<byte[]> CreateReporteDiaCasetaAsync(DiaCaseta diaCaseta);
    }
}
