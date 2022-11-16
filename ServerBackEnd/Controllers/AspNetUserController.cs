using ApiGateway.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AspNetUserController : ControllerBase
    {
        private readonly IAspNetUserService _aspNetUserService;
        public AspNetUserController(IAspNetUserService aspNetUserService)
        {
            _aspNetUserService = aspNetUserService;
        }

        [HttpGet]
        public async Task<ActionResult> GetArea()
        {
            var result = await _aspNetUserService.GetAspNetUsersAsync();
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
