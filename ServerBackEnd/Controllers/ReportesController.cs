using ApiGateway.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IReportesService _resportesService;
        public ReportesController(IReportesService resportesService)
        {
            _resportesService = resportesService;
        }

        [HttpGet("ReporteLessor/{id}")]
        public async Task<ActionResult> GetReporteLessor(int id)
        {
            var result = await _resportesService.GetReporteLessorsAsync(id);

            if (result.Succeeded)
            {
                return File(result.Content, "application/pdf", "Arrendores.pdf");
            }

            return BadRequest(result);
        }

        [HttpGet("ReporteReceptionCertificate")]
        public async Task<ActionResult> GetReporteReceptionCertificate(int IdReceptionCertificate)
        {
            var result = await _resportesService.GEetReporteReceptionCertificateAsync(IdReceptionCertificate);

            if (result.Succeeded)
            {
                return File(result.Content, "application/pdf", "Caracteristicas.pdf");
            }

            return BadRequest(result);
        }
    }
}
