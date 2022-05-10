using ApiGateway.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Security.Claims;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IdentityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Registrar nuevo usuario 
        /// </summary>
        /// <param name="createCommand">Parametros de registro</param>
        /// <returns>Regresa el usuario creado.</returns>
        /// <response code="201">Regresa el objeto creado</response>
        /// <response code="400">Alguno de los datos requeridos es incorrecto</response>
        /// <response code="500">Error por excepcion no controlada en el Gateway</response>
        [HttpPost("registro")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json", "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(UserCreateCommand createCommand)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(createCommand);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                var res = await _mediator.Send(new UserFindCommand { Email = createCommand.Email });
                return CreatedAtAction("Find", new { res.Id }, res );
            }
            return BadRequest();
        }

        /// <summary>
        /// Solicitar JWT
        /// </summary>
        /// <param name="loginCommand">Parametros de autenticación</param>
        /// <returns>Regresa el token JWT al usuario autenticado.</returns>
        /// <response code="200">Regresa el objeto solicitado</response>
        /// <response code="400">Alguno de los datos requeridos es incorrecto</response>
        /// <response code="500">Error por excepcion no controlada en el Gateway</response>
        [HttpPost("autenticar")]
        [Consumes("application/x-www-form-urlencoded")]
        [Produces("application/json", "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromForm]UserLoginCommand loginCommand)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(loginCommand);
                if (!result.Succeeded)
                {
                    return BadRequest(result);
                }
                RefreshTokenCookie(result.RefreshToken);
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        /// Solicitar nuevo JWT
        /// </summary>
        /// <returns>Reexpide nuevos tokens al usuario autenticado por refresh_token</returns>
        /// <response code="200">Regresa el objeto solicitado</response>
        /// <response code="400">Alguno de los datos requeridos es incorrecto</response>
        /// <response code="500">Error por excepcion no controlada en el Gateway</response>
        [HttpPost("refrescartoken")]
        [Produces("application/json", "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken()
        {
            if (User.Identity.IsAuthenticated)
            {
                UserLoginCommand loginCommand = new()
                {
                    Email = (User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Email))?.Value),
                    RefreshToken = Request.Cookies["refreshToken"],
                };
                var result = await _mediator.Send(loginCommand);
                if (!result.Succeeded)
                {
                    return BadRequest(result);
                }
                RefreshTokenCookie(result.RefreshToken);
                return Ok(result);
            }
            return BadRequest();

        }

        /// <summary>
        /// Consultar usuario
        /// </summary>
        /// <param name="id">Realizar busqueda por Id</param>
        /// <param name="email">Realizar busqueda por email</param>
        /// <param name="userName">Realizar busqueda por nombre de usuario</param>
        /// <returns>Regresa la información del usuario consultado</returns>
        /// <response code="200">Regresa el objeto solicitado</response>
        /// <response code="400">Alguno de los datos requeridos es incorrecto</response>
        /// <response code="404">No se pudo encontrar el objeto solicitado</response>
        /// <response code="500">Error por excepcion no controlada en el Gateway</response>
        [HttpGet("usuario")]
        [Produces("application/json", "application/problem+json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Find(string? id = null, string? email = null, string? userName = null)
        {
            if (!string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(userName))
            {
                UserFindCommand findCommand = new() { Id = id, Email = email, Username = userName };
                var result = await _mediator.Send(findCommand);
                if (!result.Succeeded)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            return BadRequest();
        }

        private void RefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
