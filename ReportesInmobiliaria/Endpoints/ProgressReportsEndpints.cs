using ReportesObra.Interfaces;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace ReportesObra.Endpoints
{
    public static class ProgressReportsEndpints
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
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapGet("/ProgressReports", async (int ? idProgressReport, int ? idBuilding, int ? idAparment, int ? idArea, int ? idElemnet, int ? idSubElement, string ? idSupervisor, IProgressReportsService _progressReportsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var progressReports = await _progressReportsService.GetProgressReportsAsync(idProgressReport, idBuilding, idAparment, idArea, idElemnet, idSubElement, idSupervisor);
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
            .WithName("GetProgressReports")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPost("/ProgressReports", async (ProgressReport progressReport, IProgressReportsService _progressReportsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var res = await _progressReportsService.CreateProgressReportsAsync(progressReport);
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
            .WithName("CreateProgressReports")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPut("/ProgressReports/{idProgressReport}", async (int idProgressReport, ProgressReport progressReport, IProgressReportsService _progressReportsService, ILogger<Program> _logger) =>
            {
                try
                {
                    if (idProgressReport != progressReport.IdProgressReport) return Results.BadRequest();
                    var res = await _progressReportsService.UpdateProgressReportsAsync(progressReport);
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
            .WithName("UpdateProgressReports")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
        }
    }
}
