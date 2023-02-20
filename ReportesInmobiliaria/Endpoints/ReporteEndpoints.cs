using ReportesObra.Interfaces;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace ReportesObra.Endpoints
{
    public static class ReporteEndpoints
    {
        public static void MapReporteEndpoints(this IEndpointRouteBuilder routes)
        {
            //routes.MapGet("/ReporteDetalles", async (int idBuilding, [FromUri] int[] idApartments, [FromUri] int[] idActivy, [FromUri] int[] idElement, [FromUri] int[]? idSubElements, IReportesService _reportesService, ILogger<Program> _logger) =>
            routes.MapPost("/DataDetallesDepartamento", async (ActivitiesDetail detallesActividad, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetDataDetallesDepartamento(detallesActividad.IdBuilding, detallesActividad.Apartments, detallesActividad.Areas, detallesActividad.Activities, detallesActividad.Elements, detallesActividad.SubElements);
                    if (newModule.Count == 0) return Results.NotFound();
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
            .WithName("GetDataDetallesDepartamento")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPost("/ReporteDetalladoPorDepartamento", async (List<DetalladoDepartamentos> detalladoDepartamentos, int? opcion, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetReporteDetallesDepartamento(detalladoDepartamentos, opcion);
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
            .WithName("GetReporteDetallesDepartamento")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPost("/DataDetallesActividad", async (ActivitiesDetail detallesActividad, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetDataDetallesActividad(detallesActividad.IdBuilding, detallesActividad.Activities, detallesActividad.Elements, detallesActividad.SubElements, detallesActividad.Apartments);
                    if (newModule.Count == 0) return Results.NotFound();
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
            .WithName("GetDataDetallesActividad")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPost("/ReporteDetalladoPorActividad", async (List<DetalladoActividades> detalladoActividades, int? opcion, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetReporteDetallesActividad(detalladoActividades, opcion);
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
            .WithName("GetReporteDetallesActividad")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPost("/ReportProgressByAparmentPDF", async (List<AparmentProgress> aparmentProgresses, IReportesService _reportesService, ILogger<Program> _logger) =>
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

            routes.MapGet("/ReportProgressByAparmentView", async (int? idAparment, IReportesService _reportesService, ILogger<Program> _logger) =>
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

            routes.MapGet("/ReportOfAparmentByActivityView", async (int? idActivity, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetActivitiesByAparment(idActivity);
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
            .WithName("GetActivitiesByAparment")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
            .AllowAnonymous();

            routes.MapPost("/ReportOfAparmentByActivityPDF", async (List<AparmentProgress> aparmentProgresses, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetReporteAvancDeActividadPorDepartamento(aparmentProgresses);
                    if (newModule == null) return Results.NoContent();
                    return Results.File(newModule,"application/pdf");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetReporteAvancDeActividadPorDepartamento")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
            .AllowAnonymous();

            routes.MapGet("/ReportProgressByActivityView", async (int? idBuilding, int? idActivity, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetActivityProgress(idBuilding, idActivity);
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
            .WithName("GetProgressByActivity")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPost("/ReportProgressByActivityPDF", async (List<ActivityProgress> aparmentProgresses, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetReporteAvanceActividad(aparmentProgresses);
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
            .WithName("GetReporteAvanceActividad")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapGet("/ReportOfActivityByAparmentView", async (int? idApartment, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetAparmentsByActivity(idApartment);
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
            .WithName("GetAparmentsByActivity")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
            .AllowAnonymous();

            routes.MapPost("/ReportOfActivityByAparmentPDF", async (List<ActivityProgressByAparment> activityProgressByAparments, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetReporteAvanceDeDepartamentoPorActividad(activityProgressByAparments);
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
            .WithName("GetReporteAvanceDeDepartamentoPorActividad")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
            .AllowAnonymous();
        }
    }
}
