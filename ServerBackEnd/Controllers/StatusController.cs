using ApiGateway.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpGet]
        public async Task<ActionResult> GetStatuses()
        {
            var result = await _statusService.GetStatusesAsync();
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{idStatus}")]
        public async Task<ActionResult> GetStatus(string idStatus)
        {
            int idInt = 0;
            if (idStatus != null)
            {
                idInt = Convert.ToInt16(idStatus);
            }

            var result = await _statusService.GetStatusAsync(idInt);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
