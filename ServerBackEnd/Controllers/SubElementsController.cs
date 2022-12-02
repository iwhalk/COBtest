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

        [HttpGet("{id}")]
        public async Task<ActionResult> GetSubElement(string id)
        {
            int idInt = 0;

            if (id != null)
            {
                idInt = Convert.ToInt16(id);
            }

            var result = await _subElementsService.GetSubElementAsync(idInt);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
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
