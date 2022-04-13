using ApiGateway.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserCreateCommand createCommand)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(createCommand);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Login(UserLoginCommand loginCommand)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(loginCommand);
                if (!result.Succeeded)
                {
                    return BadRequest("Access Denied");
                }
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
