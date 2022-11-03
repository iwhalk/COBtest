using ApiGateway.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _services;
        public ServicesController(IServicesService services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<ActionResult> GetServicesType()
        {
            var result = await _services.GetServicesAsync();

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostServices(Service service)
        {
            var result = await _services.PostServicesAsync(service);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
