﻿using ReportesObra.Interfaces;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace ReportesObra.Endpoints
{
    public static class ProgressLogsEndpints
    {
        public static void MapProgressLogsEndpints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/ProgressLog/{idProgressLog}", async (int idProgressLog, IProgressLogsService _progressLogsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var progressLogs = await _progressLogsService.GetProgressLogAsync(idProgressLog);
                    return Results.Ok(progressLogs);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetProgressLog")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapGet("/ProgressLogs", async (int? idProgressLog, int? idBuilding, int? idAparment, int? idArea, int? idElemnet, int? idSubElement, string? idSupervisor, IProgressLogsService _progressLogsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var progressLogs = await _progressLogsService.GetProgressLogsAsync(idProgressLog, idBuilding, idAparment, idArea, idElemnet, idSubElement, idSupervisor);
                    return Results.Ok(progressLogs);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetProgressLogs")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPost("/ProgressLogs", async (ProgressLog progressLog, IProgressLogsService _progressLogsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var res = await _progressLogsService.CreateProgressLogsAsync(progressLog);
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
            .WithName("CreateProgressLogs")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPut("/ProgressLogs/{idProgressLog}", async (int idProgressLog, ProgressLog progressLog, IProgressLogsService _progressLogsService, ILogger<Program> _logger) =>
            {
                try
                {
                    if (idProgressLog != progressLog.IdProgressLog) return Results.BadRequest();
                    var res = await _progressLogsService.UpdateProgressLogsAsync(progressLog);
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
            .WithName("UpdateProgressLogs")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
        }
    }
}
