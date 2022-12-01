using ReportesObra.Interfaces;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace ReportesObra.Endpoints
{
    public static class ProgressLogsEndpints
    {
        public static void MapProgressLogsEndpints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/Areas", async (IAreasService _areasService, ILogger<Program> _logger) =>
            {
                try
                {
                    var areas = await _areasService.GetAreasAsync();
                    return Results.Ok(areas);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetAreas")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapGet("/Area", async (int idArea, IAreasService _areasService, ILogger<Program> _logger) =>
            {
                try
                {
                    var area = await _areasService.GetAreaAsync(idArea);
                    return Results.Ok(area);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetArea")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPost("/Area", async (Area area, IAreasService _areasService, ILogger<Program> _logger) =>
            {
                try
                {
                    var res = await _areasService.CreateAreaAsync(area);
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
            .WithName("CreateArea")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPut("/Area/{id}", async (int id, Area area, IAreasService _areasService, ILogger<Program> _logger) =>
            {
                try
                {
                    if (id != area.IdArea) return Results.BadRequest();
                    var res = await _areasService.UpdateAreaAsync(area);
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
            .WithName("UpdateArea")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
        }
    }
}
