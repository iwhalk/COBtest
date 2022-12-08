using ReportesObra.Interfaces;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Http;

namespace ReportesObra.Endpoints
{
    public static class ReporteEndpoints
    {
        public static void MapReporteEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/ReporteDetalles", async (int idBuilding, [FromUri] int[] idApartments, [FromUri] int[] idActivy, [FromUri] int[] idElement, [FromUri] int[]? idSubElements, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetReporteDetalles(idBuilding, idApartments.ToList(), idActivy.ToList(), idElement.ToList(), idSubElements.ToList());
                    if (newModule == null) return Results.NotFound();
                    return Results.File(newModule, "application/pdf");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetReporteDetalles")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            //routes.MapGet("/ReporteDetallesPorActividad", async (int idBuilding, int idApartment, [FromUri] int[] activityIds, int ? idElement, int ? idSubElement, IReportesService _reportesService, ILogger<Program> _logger) =>
            //{
            //    try
            //    {
            //        var activities = activityIds.ToList();
            //        var newModule = await _reportesService.GetReporteDetalles(idBuilding, idApartment, activities, idElement, idSubElement);
            //        if (newModule == null) return Results.NotFound();
            //        return Results.File(newModule, "application/pdf");
            //    }
            //    catch (Exception e)
            //    {
            //        _logger.LogError(e, e.Message);
            //        if (e.GetType() == typeof(ValidationException))
            //            return Results.Problem(e.Message, statusCode: 400);
            //        return Results.Problem(e.Message);
            //    }
            //})
            //.WithName("GetReporteDetallesPorActividad")
            //.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            //.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            //.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
        }
    }
}
