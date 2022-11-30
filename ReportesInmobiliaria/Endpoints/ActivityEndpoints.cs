using ReportesObra.Interfaces;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace ReportesObra.Endpoints
{
    public static class ActivityEndpoints
    {
        public static void MapActivityEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/Activities", async (IActivitiesService _activitiesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var activities = await _activitiesService.GetActivitiesAsync();
                    return Results.Ok(activities);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetActivities")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapGet("/Activity", async (int idActivity, IActivitiesService _activitiesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var activity = await _activitiesService.GetActivityAsync(idActivity);
                    return Results.Ok(activity);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetActivity")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPost("/Activity", async (Activity activity, IActivitiesService _activitiesService, ILogger<Program> _logger) =>
            {
                try
                {
                    var res = await _activitiesService.CreateActivityAsync(activity);
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
            .WithName("CreateActivity")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPut("/Activity/{id}", async (int id, Activity activity, IActivitiesService _activitiesService, ILogger<Program> _logger) =>
            {
                try
                {
                    if (id != activity.IdActivity) return Results.BadRequest();
                    var res = await _activitiesService.UpdateActivityAsync(activity);
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
            .WithName("UpdateActivity")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
        }
    }
}
