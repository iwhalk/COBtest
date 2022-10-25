using Shared.Models;
using ReportesInmobiliaria.Services;
using ReportesInmobiliaria.Interfaces;
using Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Security.Claims;
using ReportesInmobiliaria.Repositories;
using ReportesInmobiliaria.Utilities;

namespace ReportesInmobiliaria.Services
{
    public class ReportesService : IReportesService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;
        private readonly CajeroReceptorRepository _cajeroReceptorRepository;
        private readonly TurnoCarrilesRepository _turnoCarrilesRepository;
        private readonly DiaCasetaRepository _diaCasetaRepository;
        private readonly MetodosGlbRepository _metodosGlb;
        private readonly Validaciones _validaciones;
        private readonly PdfFactory _pdfFactory;

        public ReportesService(IHttpContextAccessor httpContextAccessor, 
                               IConfiguration configuration,
                               AppDbContext dbContext,
                               CajeroReceptorRepository cajeroReceptorRepository,
                               TurnoCarrilesRepository turnoCarrilesRepository,
                               DiaCasetaRepository diaCasetaRepository,
                               Validaciones validaciones,
                               MetodosGlbRepository metodosGlb,
                               PdfFactory pdfFactory)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _cajeroReceptorRepository = cajeroReceptorRepository;
            _turnoCarrilesRepository = turnoCarrilesRepository;
            _diaCasetaRepository = diaCasetaRepository;
            _validaciones = validaciones;
            _metodosGlb = metodosGlb;
            _pdfFactory = pdfFactory;
            _configuration = configuration;
        }

        public List<Personal> GetAdministradores()
        {
            var table = new DataTable("TABLE_PERSONNEL_ADMINISTRADOR");
            string Query = "SELECT MATRICULE, rtrim(NOM)||' '||rtrim(PRENOM) AS NOMBRE FROM TABLE_PERSONNEL WHERE MATRICULE LIKE '1%%%%%' ORDER BY NOM ";
            OracleCommand Command = new(Query, _metodosGlb.GetConnectionOracle(""));
            using OracleDataAdapter adapter = new(Command);
            adapter.Fill(table);

            List<Personal> administradores = (from DataRow dr in table.Rows
                                               select new Personal()
                                               { Nombre = dr["NOMBRE"].ToString(), NumGea = dr["MATRICULE"].ToString() }).ToList();
            return administradores;
        }

        public async Task<List<TypeDelegacion>> GetDelegacionesAsync()
        {
            return await _dbContext.TypeDelegacion?.ToListAsync();
        }

        public List<Personal> GetEncargadosTurno()
        {
            var table = new DataTable("TABLE_PERSONNEL_ENCARGADOTURNO");
            string Query = "SELECT MATRICULE, rtrim(NOM)||' '||rtrim(PRENOM) AS NOMBRE FROM TABLE_PERSONNEL WHERE MATRICULE LIKE '4%%%%%' ORDER BY NOM ";

            OracleCommand Command = new(Query, _metodosGlb.GetConnectionOracle(""));
            using OracleDataAdapter adapter = new(Command);
            adapter.Fill(table);

            List<Personal> encargadosTurno = (from DataRow dr in table.Rows
                                               select new Personal()
                                               { Nombre = dr["NOMBRE"].ToString(), NumGea = dr["MATRICULE"].ToString() }).ToList();
            return encargadosTurno;
        }

        public async Task<List<TypePlaza>> GetPlazasAsync()
        {
            return await _dbContext.TypePlaza?.ToListAsync();
        }

        public KeyValuePair<string, string>[] GetTurnos()
        {
            return new Dictionary<string, string>()
                {
                    { "1", "22:00 - 06:00" },
                    { "2", "06:00 - 14:00" },
                    { "3", "14:00 - 22:00" }
                }.ToArray();
        }

        public async Task<List<Bolsa>> GetBolsasCajeroReceptorAsync(CajeroReceptor cajeroReceptor)
        {
            var turnos = GetTurnos();
            var administradores = GetAdministradores();
            //var userPlaza = await GetUserAsync();
            var delegaciones = await GetDelegacionesAsync();
            var plazas = await GetPlazasAsync();

            var delegacion = delegaciones.FirstOrDefault(x => x.NumDelegacion == cajeroReceptor.NumDelegacion);
            var plaza = plazas.FirstOrDefault(x => x.NumPlaza == cajeroReceptor.NumPlaza);
            var turno = turnos.FirstOrDefault(x => x.Key == cajeroReceptor.IdTurno);
            var administrador = administradores.FirstOrDefault(x => x.NumGea == cajeroReceptor.NumGeaAdministrador);

            string adminNum = "";
            string matriculeCajero = administrador.NumGea;
            var queryCajero = _dbContext.TypeOperadores.Where(x => x.NumGea == matriculeCajero).FirstOrDefault();

            if (queryCajero != null)
                adminNum = queryCajero.NumCapufe;
            else
                adminNum = administrador.NumGea;

            return _cajeroReceptorRepository.GetListBolsas(cajeroReceptor.Fecha, plaza.NumPlaza, turno.Value, cajeroReceptor.NumCapufeCajeroReceptor, delegacion.NomDelegacion, adminNum + "    " + administrador.Nombre, "NameConnectionString");

        }

        public async Task<Stream> CreateReporteCajeroReceptorAsync(CajeroReceptor cajeroReceptor)
        {
            if (cajeroReceptor?.IdBolsa == null || cajeroReceptor?.IdBolsa < 1)
                throw new ValidationException("IdBolsa invalido");
            var bolsas = await GetBolsasCajeroReceptorAsync(cajeroReceptor);
            var preliquidacionCajeroReceptor = _cajeroReceptorRepository.GenerarPreliquidacion_Cajero_Receptor(bolsas.FirstOrDefault(x => x.Id == cajeroReceptor.IdBolsa));
            var comparativoCajeroReceptor = _cajeroReceptorRepository.GenerarComparativo_Cajero_Receptor();
            return _pdfFactory.GenerarReporteCajeroReceptor(preliquidacionCajeroReceptor, comparativoCajeroReceptor);
        }

        public async Task<Stream> CreateReporteTurnoCarrilesAsync(TurnoCarriles model)
        {
            var turnos = GetTurnos();
            var administradores = GetAdministradores();
            var encargadosTurno = GetEncargadosTurno();
            var delegaciones = await GetDelegacionesAsync();
            var plazas = await GetPlazasAsync();

            var delegacion = delegaciones.Find(x => x.NumDelegacion == model.NumDelegacion);
            var plaza = plazas.Find(x => x.NumPlaza == model.NumPlaza);
            var turno = turnos.FirstOrDefault(x => x.Key == model.IdTurno);
            var administrador = administradores.Find(x => x.NumGea == model.NumGeaAdministrador);
            var encargadoTurno = encargadosTurno.Find(x => x.NumGea == model.NumGeaEncargadoTurno);
            //string conexionDB = "User Id=GEADBA;Password=fgeuorjvne;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.1.1.148)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=GEAPROD)))";
            string conexionDB = _configuration.GetConnectionString("Oracle");

            if (_validaciones.ValidarCarrilesCerrados(model.Fecha, model.Fecha, turno.Value, conexionDB) == "STOP")
            {
                throw new ValidationException("Existen carriles abiertos: " + _validaciones.Message);
            }
            else if (_validaciones.ValidarBolsas(model.Fecha, model.Fecha, turno.Value, conexionDB) == "STOP")
            {
                throw new ValidationException("Existen bolsas sin declarar: " + _validaciones.Message);
            }
            else if (_validaciones.ValidarComentarios(model.Fecha, model.Fecha, turno.Value, conexionDB) == "STOP")
            {
                throw new ValidationException("Falta ingresar comentarios: " + _validaciones.Message);
            }

            string encargadoNum = "";
            string adminNum = "";
            string matriculeCajero = encargadoTurno.NumGea;
            var queryCajero = _dbContext.TypeOperadores.Where(x => x.NumGea == matriculeCajero).FirstOrDefault();

            if (queryCajero != null)
                encargadoNum = queryCajero.NumCapufe;
            else
                encargadoNum = encargadoTurno.NumGea;

            matriculeCajero = administrador.NumGea;
            queryCajero = _dbContext.TypeOperadores.Where(x => x.NumGea == matriculeCajero).FirstOrDefault();

            if (queryCajero != null)
                adminNum = queryCajero.NumCapufe;
            else
                adminNum = administrador.NumGea;

            var preliquidacionTurnoCarriles = _turnoCarrilesRepository.GenerarPreliquidacion_Turno_Carriles(model.Fecha, plaza.NumPlaza, turno.Value, encargadoNum + "    " + encargadoTurno.Nombre, delegacion.NomDelegacion, adminNum + "    " + administrador.Nombre, model.Observaciones, "NameConnectionString");
            var comparativoTurnoCarriles = _turnoCarrilesRepository.GenerarComparativo_Turno_Carriles();

            return _pdfFactory.GenerarReporteTurnoCarriles(preliquidacionTurnoCarriles, comparativoTurnoCarriles);
        }

        public async Task<Stream> CreateReporteDiaCasetaAsync(DiaCaseta diaCaseta)
        {
            var administradores = GetAdministradores();
            var encargadosTurno = GetEncargadosTurno();
            var delegaciones = await GetDelegacionesAsync();
            var plazas = await GetPlazasAsync();

            var Delegacion = delegaciones.Find(x => x.NumDelegacion == diaCaseta.NumDelegacion);
            var Plaza = plazas.Find(x => x.NumPlaza == diaCaseta.NumPlaza);
            var Administrador = administradores.Find(x => x.NumGea == diaCaseta.NumGeaAdministrador);
            var EncargadoTurno = encargadosTurno.Find(x => x.NumGea == diaCaseta.NumGeaEncargadoTurno);

            string encargado_num = "";
            string admin_num = "";
            string Matricule_Cajero = EncargadoTurno.NumGea;
            var Query_Cajero = _dbContext.TypeOperadores.Where(x => x.NumGea == Matricule_Cajero).FirstOrDefault();

            if (Query_Cajero != null)
                encargado_num = Query_Cajero.NumCapufe;
            else
                encargado_num = EncargadoTurno.NumGea;

            Matricule_Cajero = Administrador.NumGea;
            Query_Cajero = _dbContext.TypeOperadores.Where(x => x.NumGea == Matricule_Cajero).FirstOrDefault();

            if (Query_Cajero != null)
                admin_num = Query_Cajero.NumCapufe;
            else
                admin_num = Administrador.NumGea;

            var preliquidacionDiaCaseta = _diaCasetaRepository.GenerarPreliquidacion_Dia_Caseta(diaCaseta.Fecha, Plaza.NumPlaza, encargado_num + "    " + EncargadoTurno.Nombre, Delegacion.NomDelegacion, admin_num + "    " + Administrador.Nombre, diaCaseta.Observaciones, "NameConnectionString");
            var comparativoDiaCaseta = _diaCasetaRepository.GenerarComparativo_Dia_Caseta();

            return _pdfFactory.GenerarReporteDiaCaseta(preliquidacionDiaCaseta, comparativoDiaCaseta);
        }

        public async Task<UsuarioPlaza> GetUsuarioPlazaAsync()
        {
            UsuarioPlaza userPlaza = new();

            var userName = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name))?.Value;
            if (userName != null && !userName.Contains('@'))
            {
                userPlaza.NumCapufe = userName;
            }
            else
            {
                var userEmail = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Email))?.Value;
                if (userEmail != null)
                {
                    userPlaza.NumCapufe = userEmail[..userEmail.LastIndexOf('@')];
                }
            }
            if (userPlaza.NumCapufe == null)
            {
                throw new ValidationException("Usuario sin numero CAPUFE.");
            }

            var operador = await _dbContext.TypeOperadores?.FirstOrDefaultAsync(x => x.NumCapufe == userPlaza.NumCapufe);
            if (operador == null)
            {
                throw new ValidationException("Operador CAPUFE invalido.");
            }
            userPlaza.NumGea = operador?.NumGea;

            var plaza = await _dbContext.TypePlaza?.FirstOrDefaultAsync(x => x.IdPlaza == operador.PlazaId);
            if (operador == null)
            {
                throw new ValidationException("Plaza invalida.");
            }
            userPlaza.NumPlaza = plaza?.NumPlaza;

            var delegacion = await _dbContext.TypeDelegacion?.FirstOrDefaultAsync(x=>x.IdDelegacion == plaza.DelegacionId);
            userPlaza.NumDelegacion = delegacion?.NumDelegacion;

            return userPlaza;
        }
    }
}
