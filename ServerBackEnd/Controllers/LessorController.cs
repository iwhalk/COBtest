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
    public class LessorController : ControllerBase
    {
        private readonly ILessorService _lessorService;
        public LessorController(ILessorService lessorService)
        {
            _lessorService = lessorService;
        }

        [HttpGet]
        public async Task<ActionResult> GetLessor()
        {
            var result = await _lessorService.GetLessorAsync();

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);  
        }

        [HttpPost]
        public async Task<ActionResult> PostLessor(Lessor lessor)
        {
            var result = await _lessorService.PostLessorAsync(lessor);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
