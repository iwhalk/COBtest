using ReportesObra.Interfaces;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace ReportesObra.Endpoints
{
    public static class ElementEndpoints
    {
        public static void MapElementEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/Elements", async (IElementsService _elementsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var elements = await _elementsService.GetElementsAsync();
                    return Results.Ok(elements);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetElements")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapGet("/Element/{id}", async (int id, IElementsService _elementsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var element = await _elementsService.GetElementAsync(id);
                    return Results.Ok(element);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetElement")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPost("/Element", async (Element element, IElementsService _elementsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var res = await _elementsService.CreateElementAsync(element);
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
            .WithName("CreateElement")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPut("/Element/{id}", async (int id, Element element, IElementsService _elementsService, ILogger<Program> _logger) =>
            {
                try
                {
                    if (id != element.IdElement) return Results.BadRequest();
                    var res = await _elementsService.UpdateElementAsync(element);
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
            .WithName("UpdateElement")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
        }
    }
}
