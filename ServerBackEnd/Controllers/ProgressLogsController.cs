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
    public class ProgressLogsController : ControllerBase
    {
        private readonly IProgressLogsService _progressLogsService;

        public ProgressLogsController(IProgressLogsService progressLogsService)
        {
            _progressLogsService = progressLogsService;
        }

        [HttpGet]
        public async Task<ActionResult> GetProgressLogs()
        {
            var result = await _progressLogsService.GetProgressLogsAsync();

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostProgressLog(ProgressLog progressLog)
        {
            var result = await _progressLogsService.PostProgressLogAsync(progressLog);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
