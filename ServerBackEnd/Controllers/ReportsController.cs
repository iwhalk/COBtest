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

        [HttpPost("DataDetallesDepartamentos")]
        public async Task<ActionResult> PostDataDetallesDepartamentos(ActivitiesDetail reporteDetalle)
        {
            var result = await _reportesService.PostDataDetallesPorDepartamentosAsync(reporteDetalle);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("ReporteDetallesDepartamentos")]
        public async Task<ActionResult> PostReporteDetallesPorDepartamento(List<DetalladoDepartamentos> detalladoDepartamentos, int? opcion)
        {
            var result = await _reportesService.PostReporteDetallesPorDepartamentosAsync(detalladoDepartamentos, opcion);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("DataDetallesActividades")]
        public async Task<ActionResult> PostDataDetallesActividades(ActivitiesDetail reporteDetalle)
        {
            var result = await _reportesService.PostDataDetallesPorActividadesAsync(reporteDetalle);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("ReporteDetallesActividades")]
        public async Task<ActionResult> PostReporteDetallesPorActividad(List<DetalladoActividades> detalladoActividades, int? opcion)
        {
            var result = await _reportesService.PostReporteDetallesPorActividadesAsync(detalladoActividades, opcion);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("ReporteEvolucion")]
        public async Task<ActionResult> PostReporteEvolucion(ActivitiesDetail reporteDetalle)
        {
            var result = await _reportesService.PostReporteEvolucionAsync(reporteDetalle);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("ReporteAvanceCosto")]
        public async Task<ActionResult> PostReporteAvanceCosto(ActivitiesDetail reporteDetalle)
        {
            var result = await _reportesService.PostReporteAvanceCostoAsync(reporteDetalle);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("ReporteAvanceTiempo")]
        public async Task<ActionResult> PostReporteAvanceTiempo(ActivitiesDetail reporteDetalle)
        {
            var result = await _reportesService.PostReporteAvanceTiempoAsync(reporteDetalle);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        //Reportes
        [HttpGet("ProgressByAparmentDataView")]
        public async Task<ActionResult> GetProgressByAparmentView(int? idBuilding, int? idAparment)
        {
            var result = await _reportesService.GetProgressByAparmentViewAsync(idBuilding, idAparment);
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
        public async Task<ActionResult> PostProgressByAparmentPDF(List<AparmentProgress> progressReport, string subTitle)
        {
            var result = await _reportesService.PostProgressByAparmentPDFAsync(progressReport, subTitle);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpGet("ProgressByActivityDataView")]
        public async Task<ActionResult> GetProgressByActivityView(int? idBuilding, int? idActivity)
        {
            var result = await _reportesService.GetProgressByActivityViewAsync(idBuilding, idActivity);
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
        public async Task<ActionResult> PostProgressByActivityPDF(List<ActivityProgress> progressReport, string subTitle)
        {
            var result = await _reportesService.PostProgressByActivityPDFAsync(progressReport, subTitle);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpGet("ProgressOfAparmentByActivityDataView")]
        public async Task<ActionResult> GetProgressOfAparmentByActivityView(int? idBuilding, int? idActivity)
        {
            var result = await _reportesService.GetProgressOfAparmentByActivityViewAsync(idBuilding, idActivity);
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

        [HttpPost("ProgressOfAparmentByActivityPDF")]
        public async Task<ActionResult> PostProgressOfAparmentByActivityPDF(List<AparmentProgress> progressReport, bool all)
        {
            var result = await _reportesService.PostProgressOfAparmentByActivityPDFAsync(progressReport, all);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("ProgressOfActivityByAparmentDataView")]
        public async Task<ActionResult> GetProgressOfActivityByAparmentView(int? idBuilding, int? idApartment)
        {
            var result = await _reportesService.GetProgressOfActivityByAparmentViewAsync(idBuilding, idApartment);
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

        [HttpPost("ProgressOfActivityByAparmentPDF")]
        public async Task<ActionResult> PostProgressOfActivityByAparmentPDF(List<ActivityProgressByAparment> progressReport, bool all)
        {
            var result = await _reportesService.PostProgressOfActivityByAparmentPDFAsync(progressReport, all);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
