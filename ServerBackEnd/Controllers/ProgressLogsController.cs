using ApiGateway.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class ProgressLogsController : ControllerBase
    {
        private readonly IProgressLogsService _progressLogsService;

        public ProgressLogsController(IProgressLogsService progressLogsService)
        {
            _progressLogsService = progressLogsService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProgressLog(string id)
        {
            int idInt = 0;

            if (id != null)
            {
                idInt = Convert.ToInt16(id);
            }

            var result = await _progressLogsService.GetProgressLogAsync(idInt);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetProgressLogs(string? idProgressLog, string? idProgressReport, string? idStatus, string? idSupervisor)
        {
            idSupervisor = GetNullableString(idSupervisor);

            int idProgressLogInt = 0;
            int idProgressReportInt = 0;
            int idStatusInt = 0;

            if (idProgressLog != null)
            {
                idProgressLogInt = Convert.ToInt16(idProgressLog);
            }
            if (idProgressReport != null)
            {
                idProgressReportInt = Convert.ToInt16(idProgressReport);
            }
            if (idStatus != null)
            {
                idStatusInt = Convert.ToInt16(idStatus);
            }

            var result = await _progressLogsService.GetProgressLogsAsync(idProgressLogInt, idProgressReportInt, idStatusInt, idSupervisor);

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

        static string? GetNullableString(string? value) => !string.IsNullOrWhiteSpace(value) && value.ToUpper().Contains("NULL") ? null : value;
    }
}
