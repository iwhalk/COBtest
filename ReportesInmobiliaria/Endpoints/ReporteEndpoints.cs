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
                    var newModule = await _reportesService.GetDataDetallesDepartamento(detallesActividad.IdBuilding, detallesActividad.Apartments, detallesActividad.Areas, detallesActividad.Activities, detallesActividad.Elements, detallesActividad.SubElements, detallesActividad.StatusOption);
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
                    var newModule = await _reportesService.GetReporteDetallesDepartamento(detalladoDepartamentos);
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
                    var newModule = await _reportesService.GetDataDetallesActividad(detallesActividad.IdBuilding, detallesActividad.Activities, detallesActividad.Elements, detallesActividad.SubElements, detallesActividad.Apartments, detallesActividad.StatusOption);
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
                    var newModule = await _reportesService.GetReporteDetallesActividad(detalladoActividades);
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

            routes.MapPost("/ReportEvolution", async (ActivitiesDetail details, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var report = await _reportesService.GetReportEvolution(details.IdBuilding, details.FechaInicio, details.FechaFin, details.Apartments,
                        details.Areas, details.Activities, details.Elements, details.SubElements, details.StatusOption, details.WithActivities);
                    if (report == null) return Results.NotFound();
                    return Results.File(report, "application/pdf");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetReportEvolution")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPost("/ReportAdvanceCost", async (ActivitiesDetail details, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var report = await _reportesService.GetReportAdvanceCost(details.IdBuilding, details.Apartments,
                        details.Areas, details.Activities, details.Elements, details.SubElements, details.StatusOption);
                    if (report == null) return Results.NotFound();
                    return Results.File(report, "application/pdf");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetReportAdvanceCost")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPost("/ReportAdvanceTime", async (ActivitiesDetail details, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var report = await _reportesService.GetReportAdvanceTime(details.IdBuilding, details.Apartments,
                        details.Areas, details.Activities, details.Elements, details.SubElements, details.StatusOption);
                    if (report == null) return Results.NotFound();
                    return Results.File(report, "application/pdf");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetReportAdvanceTime")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPost("/ReportProgressByAparmentPDF", async (List<AparmentProgress> aparmentProgresses, string subTitle, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetReporteAvance(aparmentProgresses, subTitle);
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

            routes.MapGet("/ReportProgressByAparmentView", async (int? idBuilding, int? idAparment, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetAparments(idBuilding, idAparment);
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

            routes.MapGet("/ReportGetCost", async (int? idBuilding, int? idAparment, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetAparmentTotalCost(idBuilding, idAparment);
                    if (newModule == null) return Results.NoContent();
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
          .WithName("ReportGetCost")
          .Produces<IResult>(StatusCodes.Status200OK)
          .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
          .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
          .AllowAnonymous();

            routes.MapGet("/ReportOfAparmentByActivityView", async (int? idBuilding, int? idActivity, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetActivitiesByAparment(idBuilding, idActivity);
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

            routes.MapGet("/ReportGetCostActivity", async (int? idBuilding, int? idActivity, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetActivityTotalCost(idBuilding, idActivity);
                    if (newModule == null) return Results.NoContent();
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
          .WithName("ReportGetCostActivity")
          .Produces<IResult>(StatusCodes.Status200OK)
          .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
          .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
          .AllowAnonymous();

            routes.MapPost("/ReportOfAparmentByActivityPDF", async (List<AparmentProgress> aparmentProgresses,bool all, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetReporteAvancDeActividadPorDepartamento(aparmentProgresses, all);
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

            routes.MapPost("/ReportProgressByActivityPDF", async (List<ActivityProgress> aparmentProgresses, string subTitle, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetReporteAvanceActividad(aparmentProgresses, subTitle);
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

            routes.MapGet("/ReportOfActivityByAparmentView", async (int? idBuilding, int? idApartment, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetAparmentsByActivity(idBuilding, idApartment);
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

            routes.MapPost("/ReportOfActivityByAparmentPDF", async (List<ActivityProgressByAparment> activityProgressByAparments, bool all, IReportesService _reportesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var newModule = await _reportesService.GetReporteAvanceDeDepartamentoPorActividad(activityProgressByAparments, all);
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
