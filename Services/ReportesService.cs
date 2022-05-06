using ApiGateway.Interfaces;
using ApiGateway.Models;
using ApiGateway.Proxies;
using ReportesData.Models;
using System.Text.Json;

namespace ApiGateway.Services
{
    public class ReportesService : GenericProxy, IReportesService
    {
        public ReportesService(IHttpContextAccessor httpContextAccessor,
                               IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {
        }

        public async Task<ApiResponse<byte[]>> CreateReporteCajeroReceptorAsync(CajeroReceptor cajeroReceptor)
        {
            return await PostAsync<byte[]>(cajeroReceptor, path: "reportecajeroreceptor");
            //return new ApiResponse<byte[]>(res);
        }

        public async Task<ApiResponse<byte[]>> CreateReporteDiaCasetaAsync(DiaCaseta diaCaseta)
        {
            return await PostAsync<byte[]>(diaCaseta, path: "reportediacaseta");
            //return new ApiResponse<byte[]>(res);
        }

        public async Task<ApiResponse<byte[]>> CreateReporteTurnoCarrilesAsync(TurnoCarriles turnoCarriles)
        {
            return await PostAsync<byte[]>(turnoCarriles, path: "reporteturnocarriles");
            //return new ApiResponse<byte[]>(res);
        }

        public async Task<ApiResponse<List<Bolsa>>> CreateBolsasCajeroReceptor(CajeroReceptor cajeroReceptor)
        {
            return await PostAsync<List<Bolsa>>(cajeroReceptor, path: "bolsascajeroreceptor");
        }

        public async Task<ApiResponse<List<Personal>>> GetAdministradores()
        {
            return await GetAsync<List<Personal>>(path: "administradores");
        }

        public async Task<ApiResponse<List<TypeDelegacion>>> GetDelegaciones()
        {
            return await GetAsync<List<TypeDelegacion>>(path: "delegaciones");
        }

        public async Task<ApiResponse<List<Personal>>> GetEncargadosTurno()
        {
            return await GetAsync<List<Personal>>(path: "encargadosturno");
        }

        public async Task<ApiResponse<List<TypePlaza>>> GetPlazas()
        {
            return await GetAsync<List<TypePlaza>>(path: "plazas");
        }

        public async Task<ApiResponse<KeyValuePair<string, string>>> GetTurnos()
        {
            return await GetAsync<KeyValuePair<string, string>>(path: "turnos");
        }

        public async Task<ApiResponse<UsuarioPlaza>> GetUsuarioPlazaAsync()
        {
            return await GetAsync<UsuarioPlaza>(path: "usuarioPlaza");
        }
    }
}
