using ApiGateway.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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
        public async Task<ActionResult> GetProgressReports(string? idProgressReport, string? idBuilding, string? idApartment, string? idArea, string? idElement, string? idSubElement, string? idSupervisor, bool includeProgressLogs)
        {
            idSupervisor = GetNullableString(idSupervisor);

            int idProgressReportInt = 0;
            int idBuildingInt = 0;
            int idApartmentInt = 0;
            int idAreaInt = 0;
            int idElementInt = 0;
            int idSubElementInt = 0;

            if (idProgressReport != null)
            {
                idProgressReportInt = Convert.ToInt16(idProgressReport);
            }
            if (idBuilding != null)
            {
                idBuildingInt = Convert.ToInt16(idBuilding);
            }
            if (idApartment != null)
            {
                idApartmentInt = Convert.ToInt16(idApartment);
            }
            if (idArea != null)
            {
                idAreaInt = Convert.ToInt16(idArea);
            }
            if (idElement != null)
            {
                idElementInt = Convert.ToInt16(idElement);
            }
            if (idSubElement != null)
            {
                idSubElementInt = Convert.ToInt16(idSubElement);
            }


            var result = await _progressReportService.GetProgressReportsAsync(idProgressReportInt, idBuildingInt, idApartmentInt, idAreaInt, idElementInt, idSubElementInt, idSupervisor, includeProgressLogs); 

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("ObjectAccess/{idSupervisor}")]
        public async Task<ActionResult> GetObjectAccess(string idSupervisor)
        {
            var result = await _progressReportService.GetObjectAccessAsync(idSupervisor);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("BuildingAssigned/{idSupervisor}")]
        public async Task<ActionResult> GetBuildingAssigned(string idSupervisor)
        {
            var result = await _progressReportService.GetBuildingAssignedAsync(idSupervisor);

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
