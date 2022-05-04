using ApiGateway.Interfaces;
using ApiGateway.Models;
using Microsoft.AspNetCore.Mvc;
using ReportesData.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IReportesService _reportesService;

        public ReportesController(IReportesService reportesService)
        {
            _reportesService = reportesService;
        }

        /// <summary>
        /// Obtener la informacion del usuario logueado en la plaza.
        /// </summary>
        /// <returns>Regresa un modelo con los detalles de registro del usuario autentificado.</returns>
        /// <response code="200">Regresa el objeto solicitado</response>
        /// <response code="400">Alguno de los datos requeridos es incorrecto</response>
        /// <response code="500">Error por excepcion no controlada en el Gateway</response>
        [HttpGet("usuarioPlaza")]
        [Produces("application/json", "application/problem+json")]
        [ProducesResponseType(typeof(ApiResponse<UsuarioPlaza>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetUsuarioPlaza()
        {
            return Ok(await _reportesService.GetUsuarioPlazaAsync());
        }

        // GET: api/<ReportesController>/administradores
        [HttpGet("administradores")]
        [Produces("application/json", "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Personal>>>> GetAdministradores()
        {
            return Ok(await _reportesService.GetAdministradores());
        }

        // GET: api/<ReportesController>/delegaciones
        [HttpGet("delegaciones")]
        [Produces("application/json", "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<TypeDelegacion>>>> GetDelegaciones()
        {
            return Ok(await _reportesService.GetDelegaciones());
        }

        // GET: api/<ReportesController>/encargadosTurno
        [HttpGet("encargadosTurno")]
        [Produces("application/json", "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Personal>>>> GetEncargadosTurno()
        {
            return Ok(await _reportesService.GetEncargadosTurno());
        }

        // GET: api/<ReportesController>/plazas
        [HttpGet("plazas")]
        [Produces("application/json", "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<TypePlaza>>>> GetPlazas()
        {
            return Ok(await _reportesService.GetPlazas());
        }

        // GET: api/<ReportesController>/turnos
        [HttpGet("turnos")]
        [Produces("application/json", "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<KeyValuePair<string, string>>>> GetTurnos()
        {
            return Ok(await _reportesService.GetTurnos());
        }

        // POST: api/<ReportesController>/reportecajeroreceptor
        [HttpPost("reportecajeroreceptor")]
        [Produces("application/json", "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReporteCajeroReceptor(CajeroReceptor cajeroReceptor)
        {
            var file = await _reportesService.CreateReporteCajeroReceptorAsync(cajeroReceptor);
            return File(file, "application/pdf", "ReporteCajeroReceptor.pdf");
        }

        // POST: api/<ReportesController>/reporteturnocarriles
        [HttpPost("reporteturnocarriles")]
        [Produces("application/json", "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReporteTurnoCarriles(TurnoCarriles turnoCarriles)
        {
            var file = await _reportesService.CreateReporteTurnoCarrilesAsync(turnoCarriles);
            return File(file, "application/pdf", "ReporteTurnoCarriles.pdf");
        }

        // POST: api/<ReportesController>/reportediacaseta
        [HttpPost("reportediacaseta")]
        [Produces("application/json", "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReporteDiaCaseta(DiaCaseta diaCaseta)
        {
            var file = await _reportesService.CreateReporteDiaCasetaAsync(diaCaseta);
            return File(file, "application/pdf", "ReporteDiaCaseta.pdf");
        }

    }
}
