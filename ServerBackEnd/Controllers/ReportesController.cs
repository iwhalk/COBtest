using ApiGateway.Interfaces;
using ApiGateway.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IReportesService _reportesService;

        public ReportesController(IReportesService reportesService)
        {
            _reportesService = reportesService;
        }

        [HttpPost("Detalles")]
        public async Task<ActionResult> PostReporteDetalles(ActivitiesDetail reporteDetalle)
        {
            var result = await _reportesService.PostReporteDetallesAsync(reporteDetalle);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("DetallesPorActividad")]
        public async Task<ActionResult> PostReporteDetallesPorActividad(ActivitiesDetail reporteDetalle)
        {
            var result = await _reportesService.PostReporteDetallesPorActividadesAsync(reporteDetalle);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
