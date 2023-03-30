using ReportesObra.Interfaces;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace ReportesObra.Endpoints
{
    public static class ProgressReportsEndpoints
    {
        public static void MapProgressReportsEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/ProgressReport/{idProgressReport}", async (int idProgressReport, IProgressReportsService _progressReportsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var progressReports = await _progressReportsService.GetProgressReportAsync(idProgressReport);
                    return Results.Ok(progressReports);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetProgressReport")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
            .AllowAnonymous();

            routes.MapGet("/ObjectsAccess/{idSupervisor}", async (string idSupervisor, IProgressReportsService _progressReportsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var progressReports = await _progressReportsService.GetObjectsAccessAsync(idSupervisor);
                    return Results.Ok(progressReports);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetObjectsAccessAsync")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapGet("/BuildingAssigned/{idSupervisor}", async (string idSupervisor, IProgressReportsService _progressReportsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var progressReports = await _progressReportsService.GetIdBuildingAssigned(idSupervisor);
                    return Results.Ok(progressReports);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetIdBuildingAssignedAsync")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapGet("/ProgressReports", async (int? idProgressReport, int? idBuilding, int? idApartment, int? idArea, int? idElement, int? idSubElement, string? idSupervisor, bool includeProgressLogs, IProgressReportsService _progressReportsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var progressReports = await _progressReportsService.GetProgressReportsAsync(idProgressReport, idBuilding, idApartment, idArea, idElement, idSubElement, idSupervisor, includeProgressLogs);
                    return Results.Ok(progressReports);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetProgressReportS")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
            .AllowAnonymous();

            routes.MapPost("/xxx", async (ActivitiesDetail XXX, IProgressReportsService _progressReportsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var progressReports = await _progressReportsService.GetProgressReportsDetailedAsync(XXX.IdBuilding, XXX.Apartments, XXX.Areas, XXX.Elements, XXX.SubElements, XXX.Activities);
                    return Results.Ok(progressReports);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("Getxxx")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
            .AllowAnonymous();

            routes.MapPost("/ProgressReport", async (ProgressReport progressReport, IProgressReportsService _progressReportsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var res = await _progressReportsService.CreateProgressReportAsync(progressReport);
                    return Results.Ok(res);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("CreateProgressReport")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
            .AllowAnonymous();

            routes.MapPut("/ProgressReport/{idProgressReport}", async (int idProgressReport, ProgressReport progressReport, IProgressReportsService _progressReportsService, ILogger<Program> _logger) =>
            {
                try
                {
                    if (idProgressReport != progressReport.IdProgressReport) return Results.BadRequest();
                    var res = await _progressReportsService.UpdateProgressReportAsync(progressReport);
                    return Results.Ok(res);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("UpdateProgressReport")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
            .AllowAnonymous();
        }
    }
}
