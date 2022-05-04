using ApiGateway.Interfaces;
using ApiGateway.Models;
using ApiGateway.Proxies;
using ReportesData.Models;

namespace ApiGateway.Services
{
    public class ReportesService : GenericProxy, IReportesService
    {
        public ReportesService(IHttpContextAccessor httpContextAccessor,
                               IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {
        }

        public async Task<byte[]> CreateReporteCajeroReceptorAsync(CajeroReceptor cajeroReceptor)
        {
            var res = await PostStreamAsync(cajeroReceptor, path: "reportecajeroreceptor");
            return res;
        }

        public async Task<byte[]> CreateReporteDiaCasetaAsync(DiaCaseta diaCaseta)
        {
            var res = await PostStreamAsync(diaCaseta, path: "reportediacaseta");
            return res;
        }

        public async Task<byte[]> CreateReporteTurnoCarrilesAsync(TurnoCarriles turnoCarriles)
        {
            var res = await PostStreamAsync(turnoCarriles, path: "reporteturnocarriles");
            return res;
        }

        public async Task<ApiResponse<List<Personal>>> GetAdministradores()
        {
            var res = await GetAsync<List<Personal>>(path: "administradores");
            return new ApiResponse<List<Personal>>(res);
        }

        public async Task<ApiResponse<List<TypeDelegacion>>> GetDelegaciones()
        {
            var res = await GetAsync<List<TypeDelegacion>>(path: "delegaciones");
            return new ApiResponse<List<TypeDelegacion>>(res);
        }

        public async Task<ApiResponse<List<Personal>>> GetEncargadosTurno()
        {
            var res = await GetAsync<List<Personal>>(path: "encargadosturno");
            return new ApiResponse<List<Personal>>(res);
        }

        public async Task<ApiResponse<List<TypePlaza>>> GetPlazas()
        {
            var res = await GetAsync<List<TypePlaza>>(path: "plazas");
            return new ApiResponse<List<TypePlaza>>(res);
        }

        public async Task<ApiResponse<KeyValuePair<string, string>>> GetTurnos()
        {
            var res = await GetAsync<KeyValuePair<string, string>>(path: "turnos");
            return new ApiResponse<KeyValuePair<string, string>>(res);
        }

        public async Task<ApiResponse<UsuarioPlaza>> GetUsuarioPlazaAsync()
        {
            var res = await GetAsync<UsuarioPlaza>(path: "usuarioPlaza");
            return new ApiResponse<UsuarioPlaza>(res);
        }
    }
}
