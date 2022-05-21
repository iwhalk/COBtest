using ReportesData.Models;
using TestingFrontEnd.Interfaces;
using TestingFrontEnd.Stores;

namespace TestingFrontEnd.Services
{
    public class ReportesService : IReportesService
    {
        private readonly ApplicationContext _context;
        private readonly IGenericRepository _repository;

        public ReportesService(ApplicationContext context, IGenericRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<List<Bolsa>> CreateBolsasCajeroReceptorAsync(CajeroReceptor cajeroReceptor)
        {
            return await _repository.PostAsync<List<Bolsa>>("api/reportes/bolsascajeroreceptor", cajeroReceptor);
        }

        public async Task<byte[]> CreateReporteCajeroReceptorAsync(CajeroReceptor cajeroReceptor)
        {
            return await _repository.PostAsync<byte[]>("api/reportes/reportecajeroreceptor", cajeroReceptor);
        }

        public async Task<byte[]> CreateReporteDiaCasetaAsync(DiaCaseta diaCaseta)
        {
            return await _repository.PostAsync<byte[]>("api/reportes/reportediacaseta", diaCaseta);
        }

        public async Task<byte[]> CreateReporteTurnoCarrilesAsync(TurnoCarriles turnoCarriles)
        {
            return await _repository.PostAsync<byte[]>("api/reportes/reporteturnocarriles", turnoCarriles);
        }

        public async Task<List<Personal>> GetAdministradoresAsync()
        {
            if (_context.Administradores == null)
            {
                var response = await _repository.GetAsync<List<Personal>>("api/reportes/administradores");
                if (response != null)
                {
                    _context.Administradores = response;
                    return _context.Administradores;
                }
            }
            return _context.Administradores;
        }

        public async Task<List<TypeDelegacion>> GetDelegacionesAsync()
        {
            if (_context.Delegaciones == null)
            {
                var response = await _repository.GetAsync<List<TypeDelegacion>>("api/reportes/delegaciones");
                if (response != null)
                {
                    _context.Delegaciones = response;
                }
            }
            return _context.Delegaciones;
        }

        public async Task<List<Personal>> GetEncargadosTurnoAsync()
        {
            if (_context.EncargadosTurno == null)
            {
                var response = await _repository.GetAsync<List<Personal>>("api/reportes/encargadosturno");
                if (response != null)
                {
                    _context.EncargadosTurno = response;
                }
            }
            return _context.EncargadosTurno;
        }

        public async Task<List<TypePlaza>> GetPlazasAsync()
        {
            if (_context.Plazas == null)
            {
                var response = await _repository.GetAsync<List<TypePlaza>>("api/reportes/plazas");
                if (response != null)
                {
                    _context.Plazas = response;
                }
            }
            return _context.Plazas;
        }

        public async Task<KeyValuePair<string, string>[]> GetTurnosAsync()
        {
            if (_context.EncargadosTurno == null)
            {
                var response = await _repository.GetAsync<KeyValuePair<string, string>[]>("api/reportes/turnos");
                if (response != null)
                {
                    _context.Turnos = response;
                }
            }
            return _context.Turnos;
        }

        public async Task<UsuarioPlaza> GetUsuarioPlazaAsync()
        {
            if (_context.UsuarioPlaza == null)
            {
                var response = await _repository.GetAsync<UsuarioPlaza>("api/reportes/usuarioPlaza");
                if (response != null)
                {
                    _context.UsuarioPlaza = response;
                }
            }
            return _context.UsuarioPlaza;
        }
    }
}
