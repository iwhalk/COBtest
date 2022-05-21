using ApiGateway.Interfaces;
using ApiGateway.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportesData.Models;
using Shared;
using System.Net.Mime;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IReportesService _reportesService;

        public ReportesController(IReportesService reportesService)
        {
            _reportesService = reportesService;
        }

        // GET: api/<ReportesController>/usuarioPlaza
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
        public async Task<ActionResult<ApiResponse<UsuarioPlaza>>> GetUsuarioPlaza()
        {
            return Ok(await _reportesService.GetUsuarioPlazaAsync());
        }

        // GET: api/<ReportesController>/administradores
        /// <summary>
        /// Obtener la informacion de los administradores registrados en la plaza.
        /// </summary>
        /// <returns>Regresa un arreglo con los detalles de los administradores.</returns>
        /// <response code="200">Regresa el arreglo de objetos solicitado</response>
        /// <response code="400">Alguno de los datos requeridos es incorrecto</response>
        /// <response code="500">Error por excepcion no controlada en el Gateway</response>
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
        /// <summary>
        /// Obtener la informacion de las delegaciones registradas en la plaza.
        /// </summary>
        /// <returns>Regresa un arreglo con los detalles de las delegaciones.</returns>
        /// <response code="200">Regresa el arreglo de objetos solicitado</response>
        /// <response code="400">Alguno de los datos requeridos es incorrecto</response>
        /// <response code="500">Error por excepcion no controlada en el Gateway</response>
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
        /// <summary>
        /// Obtener la informacion de los encargados de turno registrados en la plaza.
        /// </summary>
        /// <returns>Regresa un arreglo con los detalles de los encargados de turno.</returns>
        /// <response code="200">Regresa el arreglo de objetos solicitado</response>
        /// <response code="400">Alguno de los datos requeridos es incorrecto</response>
        /// <response code="500">Error por excepcion no controlada en el Gateway</response>
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
        /// <summary>
        /// Obtener la informacion de las plazas registradas en la plaza.
        /// </summary>
        /// <returns>Regresa un arreglo con los detalles de las plazas.</returns>
        /// <response code="200">Regresa el arreglo de objetos solicitado</response>
        /// <response code="400">Alguno de los datos requeridos es incorrecto</response>
        /// <response code="500">Error por excepcion no controlada en el Gateway</response>
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
        /// <summary>
        /// Obtener la informacion de los turnos registrados en la plaza.
        /// </summary>
        /// <returns>Regresa un arreglo con los detalles de los turnos.</returns>
        /// <response code="200">Regresa el arreglo de objetos solicitado</response>
        /// <response code="400">Alguno de los datos requeridos es incorrecto</response>
        /// <response code="500">Error por excepcion no controlada en el Gateway</response>
        [HttpGet("turnos")]
        [Produces("application/json", "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<KeyValuePair<string, string>[]>>> GetTurnos()
        {
            return Ok(await _reportesService.GetTurnos());
        }

        // GET: api/<ReportesController>/bolsascajeroreceptor
        /// <summary>
        /// Obtener la informacion de las plazas registradas en la plaza.
        /// </summary>
        /// <returns>Regresa un arreglo con los detalles de las plazas.</returns>
        /// <response code="200">Regresa el arreglo de objetos solicitado</response>
        /// <response code="400">Alguno de los datos requeridos es incorrecto</response>
        /// <response code="500">Error por excepcion no controlada en el Gateway</response>
        [HttpPost("bolsascajeroreceptor")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json", "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Bolsa>>>> CreateBolsasCajeroReceptor(CajeroReceptor cajeroReceptor)
        {
            return Ok(await _reportesService.CreateBolsasCajeroReceptor(cajeroReceptor));
        }

        // POST: api/<ReportesController>/reportecajeroreceptor
        /// <summary>
        /// Obtener el reporte de cajero en formato pdf.
        /// </summary>
        /// <returns>Regresa un stream de datos "application/pdf" del reporte solicitado.</returns>
        /// <response code="200">Regresa el reporte solicitado</response>
        /// <response code="400">Alguno de los datos requeridos es incorrecto</response>
        /// <response code="500">Error por excepcion no controlada en el Gateway</response>
        [HttpPost("reportecajeroreceptor")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/pdf", "application/json", "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReporteCajeroReceptor(CajeroReceptor cajeroReceptor)
        {
            var response = await _reportesService.CreateReporteCajeroReceptorAsync(cajeroReceptor);
            if (response.Succeeded && response.Content != null)
                return File(response.Content, "application/pdf", "ReporteCajeroReceptor.pdf");
            return Ok(response);
        }

        // POST: api/<ReportesController>/reporteturnocarriles
        /// <summary>
        /// Obtener el reporte de carriles en formato pdf.
        /// </summary>
        /// <returns>Regresa un stream de datos "application/pdf" del reporte solicitado.</returns>
        /// <response code="200">Regresa el reporte solicitado</response>
        /// <response code="400">Alguno de los datos requeridos es incorrecto</response>
        /// <response code="500">Error por excepcion no controlada en el Gateway</response>
        [HttpPost("reporteturnocarriles")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/pdf", "application/json", "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReporteTurnoCarriles(TurnoCarriles turnoCarriles)
        {
            var response = await _reportesService.CreateReporteTurnoCarrilesAsync(turnoCarriles);
            if (response.Succeeded)
                return File(response.Content, "application/pdf", "ReporteTurnoCarriles.pdf");
            return Ok(response);
        }

        // POST: api/<ReportesController>/reportediacaseta
        /// <summary>
        /// Obtener el reporte del dia por caseta en formato pdf.
        /// </summary>
        /// <returns>Regresa un stream de datos "application/pdf" del reporte solicitado.</returns>
        /// <response code="200">Regresa el reporte solicitado</response>
        /// <response code="400">Alguno de los datos requeridos es incorrecto</response>
        /// <response code="500">Error por excepcion no controlada en el Gateway</response>
        [HttpPost("reportediacaseta")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/pdf", "application/json", "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReporteDiaCaseta(DiaCaseta diaCaseta)
        {
            var response = await _reportesService.CreateReporteDiaCasetaAsync(diaCaseta);
            if (response.Succeeded)
                return File(response.Content, "application/pdf", "ReporteDiaCaseta.pdf");
            return Ok(response);
        }

    }
}
