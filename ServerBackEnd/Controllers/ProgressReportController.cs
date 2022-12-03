using ApiGateway.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class ProgressReportController : ControllerBase
    {
        private readonly IProgressReportService _progressReportService;

        public ProgressReportController(IProgressReportService progressReportService)
        {
            _progressReportService = progressReportService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProgressReport(string id)
        {
            int idInt = 0;

            if (id != null)
            {
                idInt = Convert.ToInt16(id);
            }

            var result = await _progressReportService.GetProgressReportAsync(idInt);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetProgressReports(string? idProgressReport, string? idBuilding, string? idAparment, string? idArea, string? idElemnet, string? idSubElement, string? idSupervisor)
        {
            idSupervisor = GetNullableString(idSupervisor);

            int idProgressReportInt = 0;
            int idBuildingInt = 0;
            int idAparmentInt = 0;
            int idAreaInt = 0;
            int idElemnetInt = 0;
            int idSubElementInt = 0;

            if (idProgressReport != null)
            {
                idProgressReportInt = Convert.ToInt16(idProgressReport);
            }
            if (idBuilding != null)
            {
                idBuildingInt = Convert.ToInt16(idBuilding);
            }
            if (idAparment != null)
            {
                idAparmentInt = Convert.ToInt16(idAparment);
            }
            if (idArea != null)
            {
                idAreaInt = Convert.ToInt16(idArea);
            }
            if (idElemnet != null)
            {
                idElemnetInt = Convert.ToInt16(idElemnet);
            }
            if (idSubElement != null)
            {
                idSubElementInt = Convert.ToInt16(idSubElement);
            }


            var result = await _progressReportService.GetProgressReportsAsync(idProgressReportInt, idBuildingInt, idAparmentInt, idAreaInt, idElemnetInt, idSubElementInt, idSupervisor); 

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostProgressReport(ProgressReport progressReport)
        {
            var result = await _progressReportService.PostProgressReportAsync(progressReport);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        static string? GetNullableString(string? value) => !string.IsNullOrWhiteSpace(value) && value.ToUpper().Contains("NULL") ? null : value;
    }
}
