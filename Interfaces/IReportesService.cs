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
        Task<ApiResponse<List<Bolsa>>> CreateBolsasCajeroReceptor(CajeroReceptor cajeroReceptor);
        Task<ApiResponse<byte[]>> CreateReporteCajeroReceptorAsync(CajeroReceptor cajeroReceptor);
        Task<ApiResponse<byte[]>> CreateReporteTurnoCarrilesAsync(TurnoCarriles turnoCarriles);
        Task<ApiResponse<byte[]>> CreateReporteDiaCasetaAsync(DiaCaseta diaCaseta);
    }
}
