using ReportesData.Models;
using TestingFrontEnd.Interfaces;
using TestingFrontEnd.Stores;
using System.Net.Http.Json;
using TestingFrontEnd.Models;

namespace TestingFrontEnd.Services
{
    public class ReportesService : IReportesService
    {
        private readonly ApplicationContext _context;
        private readonly HttpClient _httpClient;

        public ReportesService(ApplicationContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<List<Bolsa>> CreateBolsasCajeroReceptorAsync(CajeroReceptor cajeroReceptor)
        {
            var response = await _httpClient.PostAsJsonAsync("api/reportes/bolsascajeroreceptor", cajeroReceptor);
            if (response != null && response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<Bolsa>>>();
                return apiResponse.Content;
            }
            return null;
        }

        public async Task<byte[]> CreateReporteCajeroReceptorAsync(CajeroReceptor cajeroReceptor)
        {
            var response = await _httpClient.PostAsJsonAsync("api/reportes/reportecajeroreceptor", cajeroReceptor);
            if (response != null && response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsByteArrayAsync();
                return apiResponse;
            }
            return null;
        }

        public Task<byte[]> CreateReporteDiaCasetaAsync(DiaCaseta diaCaseta)
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> CreateReporteTurnoCarrilesAsync(TurnoCarriles turnoCarriles)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Personal>> GetAdministradoresAsync()
        {
            if (_context.Administradores == null)
            {
                var response = await _httpClient.GetAsync("api/reportes/administradores");
                if (response != null && response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<Personal>>>();
                    _context.Administradores = apiResponse.Content;
                }
            }
            return _context.Administradores;
        }

        public async Task<List<TypeDelegacion>> GetDelegacionesAsync()
        {
            if (_context.Delegaciones == null)
            {
                var response = await _httpClient.GetAsync("api/reportes/delegaciones");
                if (response != null && response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<TypeDelegacion>>>();
                    _context.Delegaciones = apiResponse.Content;
                }
            }
            return _context.Delegaciones;
        }

        public async Task<List<Personal>> GetEncargadosTurnoAsync()
        {
            if (_context.EncargadosTurno == null)
            {
                var response = await _httpClient.GetAsync("api/reportes/encargadosturno");
                if (response != null && response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<Personal>>>();
                    _context.EncargadosTurno = apiResponse.Content;
                }
            }
            return _context.EncargadosTurno;
        }

        public async Task<List<TypePlaza>> GetPlazasAsync()
        {
            if (_context.Plazas == null)
            {
                var response = await _httpClient.GetAsync("api/reportes/plazas");
                if (response != null && response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<TypePlaza>>>();
                    _context.Plazas = apiResponse.Content;
                }
            }
            return _context.Plazas;
        }

        public async Task<KeyValuePair<string, string>[]> GetTurnosAsync()
        {
            if (_context.EncargadosTurno == null)
            {
                var response = await _httpClient.GetAsync("api/reportes/turnos");
                if (response != null && response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<KeyValuePair<string, string>[]>>();
                    _context.Turnos = apiResponse.Content;
                }
            }
            return _context.Turnos;
        }

        public async Task<UsuarioPlaza> GetUsuarioPlazaAsync()
        {
            if (_context.UsuarioPlaza == null)
            {
                var response = await _httpClient.GetAsync("api/reportes/usuarioPlaza");
                if (response != null && response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UsuarioPlaza>>();
                    _context.UsuarioPlaza = apiResponse.Content;
                }
            }
            return _context.UsuarioPlaza;
        }
    }
}
