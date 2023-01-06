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
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService _reportesService;

        public ReportsController(IReportsService reportesService)
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

        //Reportes
        [HttpGet("ProgressByAparmentDataView")]
        public async Task<ActionResult> GetProgressByAparmentView(int? id)
        {
            var result = await _reportesService.GetProgressByAparmentViewAsync(id);
            if (result != null)
            {
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest(result);
        }

        [HttpPost("ProgressByAparmentPDF")]
        public async Task<ActionResult> PostProgressByAparmentPDF(List<AparmentProgress> progressAparmentList)
        {
            var result = await _reportesService.PostProgressByAparmentPDFAsync(progressAparmentList);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpGet("ProgressByActivityDataView")]
        public async Task<ActionResult> GetProgressByActivityView(int? id)
        {
            var result = await _reportesService.GetProgressByAparmentViewAsync(id);
            if (result != null)
            {
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest(result);
        }

        [HttpPost("ProgressByActivityPDF")]
        public async Task<ActionResult> PostProgressByActivityPDF(List<AparmentProgress> progressAparmentList)
        {
            var result = await _reportesService.PostProgressByAparmentPDFAsync(progressAparmentList);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
