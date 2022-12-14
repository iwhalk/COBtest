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
            //routes.MapGet("/ReporteDetalles", async (int idBuilding, [FromUri] int[] idApartments, [FromUri] int[] idActivy, [FromUri] int[] idElement, [FromUri] int[]? idSubElements, IReportesService _reportesService, ILogger<Program> _logger) =>
            routes.MapPost("/ReporteDetalles", async (DetallesActividad detallesActividad, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetReporteDetalles(detallesActividad.idBuilding, detallesActividad.idApartments, detallesActividad.idActivy, detallesActividad.idElement, detallesActividad.idSubElements);
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

            routes.MapPost("/ReporteDetalladoPorActividad", async (DetailsActivity detailsActivity, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetReporteDetallesActividad(detailsActivity.idBuilding, detailsActivity.idActivities, detailsActivity.idElements, detailsActivity.idApartments);
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
            .WithName("GetReporteDetalladoActividad")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPost("/ReporteAvance", async (List<AparmentProgress> aparmentProgresses, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetReporteAvance(aparmentProgresses);
                    if (newModule == null) return Results.NoContent();
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
            .WithName("GetReporteAvance")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
            .AllowAnonymous();

            routes.MapGet("/ReporteAvanceVista", async (int? idAparment, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetAparments(idAparment);
                    if (newModule.Count == 0) return Results.NoContent();                    
                    return Results.Ok(newModule);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetReporteAvanceVista")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
            .AllowAnonymous();


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
