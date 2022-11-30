using ApiGateway.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ApartmentsController : ControllerBase
    {
        private readonly IApartmentsService _aparmentsService;

        public ApartmentsController(IApartmentsService apartmentsService)
        {
            _aparmentsService = apartmentsService;
        }

        [HttpGet]
        public async Task<ActionResult> GetApartments()
        {
            var result = await _aparmentsService.GetApartmentsAsync();

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostApartment(Apartment apartment)
        {
            var result = await _aparmentsService.PostApartmentAsync(apartment);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
