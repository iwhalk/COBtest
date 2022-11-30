using ApiGateway.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SubElementsController : ControllerBase
    {
        private readonly ISubElementsService _subElementsService;

        public SubElementsController(ISubElementsService subElementsService)
        {
            _subElementsService = subElementsService;
        }

        [HttpGet]
        public async Task<ActionResult> GetSubElements()
        {
            var result = await _subElementsService.GetSubElementsAsync();

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostSubElement(SubElement subElement)
        {
            var result = await _subElementsService.PostSubElementAsync(subElement);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
