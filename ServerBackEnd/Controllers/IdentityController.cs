using ApiGateway.Models;
using ApiGateway.Services;
using AspNet.Security.OpenIdConnect.Primitives;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Net.Mime;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : Controller
    {
        private readonly IMediator _mediator;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IdentityController(IMediator mediator, SignInManager<ApplicationUser> signInManager)
        {
            _mediator = mediator;
            _signInManager = signInManager;
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

        [HttpPost("token")]
        public async Task<IActionResult> ExchangeAsync()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                          throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            ClaimsPrincipal claimsPrincipal;

            ForbidResult? forbidResult = new(authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            if (request.IsClientCredentialsGrantType())
            {
                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                identity.AddClaim(Claims.Subject, request.ClientId ?? throw new InvalidOperationException());

                claimsPrincipal = new ClaimsPrincipal(identity);

                claimsPrincipal.SetScopes(request.GetScopes());
            }
            else if (request.IsAuthorizationCodeGrantType())
            {
                var authorization = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme); 
                if (!authorization.Succeeded)
                {
                    forbidResult.Properties = new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidRequest,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = authorization.Failure?.Message
                    });
                    return forbidResult;
                }

                claimsPrincipal = authorization.Principal;
            }
            else if (request.IsPasswordGrantType())
            {

                UserLoginCommand loginCommand = new()
                {
                    UserName = request.Username,
                    Password = request.Password,
                    RefreshToken = Request.Cookies["refreshToken"],
                };
                var identity = await _mediator.Send(loginCommand);
                if (!identity.Succeeded)
                {
                    forbidResult.Properties = new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidRequest,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = identity.ErrorDescription
                    });
                    return forbidResult;
                }
                claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(identity.User);
                claimsPrincipal.SetScopes(request.GetScopes());
            }
            else
            {
                throw new InvalidOperationException("The specified grant type is not supported.");
            }

            foreach (var claim in claimsPrincipal.Claims)
            {
                claim.SetDestinations(GetDestinations(claim, claimsPrincipal));
            }

            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [HttpGet("authorize")]
        [HttpPost("authorize")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Authorize()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            var authenticate = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);

            if (!authenticate.Succeeded)
            {
                return Challenge(
                    authenticationSchemes: IdentityConstants.ApplicationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                            Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                    });
            }

            UserLoginCommand loginCommand = new()
            {
                UserName = authenticate.Principal.Identity?.Name,
                RefreshToken = Request.Cookies["refreshToken"],
            };

            var identity = await _mediator.Send(loginCommand);
            if (!identity.Succeeded)
            {
                ForbidResult? forbidResult = new(authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)
                {
                    Properties = new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidRequest,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = identity.ErrorDescription
                    })
                };
                return forbidResult;
            }

            ClaimsPrincipal claimsPrincipal;
            claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(identity.User);
            claimsPrincipal.SetScopes(request.GetScopes());

            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [HttpGet("logout")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Logout() => View();

        [HttpPost("logout"), ActionName(nameof(Logout)), ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutPost()
        {
            await _signInManager.SignOutAsync();

            return SignOut(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = "/"
                });
        }

        /// <summary>
        /// Solicitar JWT
        /// </summary>
        /// <param name="loginCommand">Parametros de autenticación</param>
        /// <returns>Regresa el token JWT al usuario autenticado.</returns>
        /// <response code="200">Regresa el objeto solicitado</response>
        /// <response code="400">Alguno de los datos requeridos es incorrecto</response>
        /// <response code="500">Error por excepcion no controlada en el Gateway</response>
        //[HttpPost("auth")]
        //[Consumes("application/x-www-form-urlencoded")]
        //[Produces("application/json", "application/problem+json")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> Login([FromForm]UserLoginCommand loginCommand)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _mediator.Send(loginCommand);
        //        if (!result.Succeeded)
        //        {
        //            return BadRequest(result);
        //        }
        //        RefreshTokenCookie(result.RefreshToken);
        //        return Ok(result);
        //    }
        //    return BadRequest();
        //}

        /// <summary>
        /// Solicitar nuevo JWT
        /// </summary>
        /// <returns>Reexpide nuevos tokens al usuario autenticado por refresh_token</returns>
        /// <response code="200">Regresa el objeto solicitado</response>
        /// <response code="400">Alguno de los datos requeridos es incorrecto</response>
        /// <response code="500">Error por excepcion no controlada en el Gateway</response>
        //[HttpPost("refreshtoken")]
        //[Produces("application/json", "application/problem+json")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> RefreshToken()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        UserLoginCommand loginCommand = new()
        //        {
        //            Email = (User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Email))?.Value),
        //            RefreshToken = Request.Cookies["refreshToken"],
        //        };
        //        var result = await _mediator.Send(loginCommand);
        //        if (!result.Succeeded)
        //        {
        //            return BadRequest(result);
        //        }
        //        RefreshTokenCookie(result.RefreshToken);
        //        return Ok(result);
        //    }
        //    return BadRequest();

        //}

        private void RefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
        private IEnumerable<string> GetDestinations(Claim claim, ClaimsPrincipal principal)
        {
            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            switch (claim.Type)
            {
                case Claims.Name:
                    yield return Destinations.AccessToken;

                    if (principal.HasScope(Scopes.Profile))
                        yield return Destinations.IdentityToken;

                    yield break;

                case Claims.Email:
                    yield return Destinations.AccessToken;

                    if (principal.HasScope(Scopes.Email))
                        yield return Destinations.IdentityToken;

                    yield break;

                case Claims.Role:
                    yield return Destinations.AccessToken;

                    if (principal.HasScope(Scopes.Roles))
                        yield return Destinations.IdentityToken;

                    yield break;

                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                case "AspNet.Identity.SecurityStamp": yield break;

                default:
                    yield return Destinations.AccessToken;
                    yield break;
            }
        }
    }
}
