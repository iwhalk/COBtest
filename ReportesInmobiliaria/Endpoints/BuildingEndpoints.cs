using ReportesObra.Interfaces;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace ReportesObra.Endpoints
{
    public static class BuildingEndpoints
    {
        public static void MapBuildingEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/Buildings", async (IBuildingsService _buildingsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var buildings = await _buildingsService.GetBuildingsAsync();
                    return Results.Ok(buildings);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetBuildings")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapGet("/Building", async (int idBuilding, IBuildingsService _buildingsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var building = await _buildingsService.GetBuildingAsync(idBuilding);
                    return Results.Ok(building);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetBuilding")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPost("/Building", async (Building building, IBuildingsService _buildingsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var res = await _buildingsService.CreateBuildingAsync(building);
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
            .WithName("CreateBuilding")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPut("/Building/{id}", async (int id, Building building, IBuildingsService _buildingsService, ILogger<Program> _logger) =>
            {
                try
                {
                    if (id != building.IdBuilding) return Results.BadRequest();
                    var res = await _buildingsService.UpdateBuildingAsync(building);
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
            .WithName("UpdateBuilding")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
        }
    }
}
