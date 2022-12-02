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
    public class ElementsController : ControllerBase
    {
        private readonly IElementsService _elementsService;

        public ElementsController(IElementsService elementsService)
        {
            _elementsService = elementsService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetElement(string id)
        {
            int idInt = 0;

            if (id != null)
            {
                idInt = Convert.ToInt16(id);
            }

            var result = await _elementsService.GetElementAsync(idInt);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetElements()
        {
            var result = await _elementsService.GetElementsAsync();

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostElement(Element element)
        {
            var result = await _elementsService.PostElementAsync(element);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
