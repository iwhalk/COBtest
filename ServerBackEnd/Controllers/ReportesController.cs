using ApiGateway.Interfaces;
using ApiGateway.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IReportesService _reportesService;

        public ReportesController(IReportesService reportesService)
        {
            _reportesService = reportesService;
        }

        [HttpPost]
        public async Task<ActionResult> PostReporteDetalles(ReporteDetalle reporteDetalle)
        {
            var result = await _reportesService.PostReporteDetallesAsync(reporteDetalle);

            if (result.Succeeded)
            {
                return File(result.Content, "application/pdf", "ReportesDetalles.pdf");
            }

            return BadRequest(result);
        }
    }
}
