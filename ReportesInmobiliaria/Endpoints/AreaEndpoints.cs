using ReportesObra.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ReportesObra.Endpoints
{
    public static class AreaEndpoints
    {
        public static void MapAreaEndpoints(this IEndpointRouteBuilder routes) 
        {
            //    routes.MapGet("/Areas", async (IAreasService _areasService, ILogger<Program> _logger) =>
            //    {
            //        try
            //        {
            //            var areas = await _areasService.GetAreasAsync();
            //            return Results.Ok(areas);
            //        }
            //        catch (Exception e)
            //        {
            //            _logger.LogError(e, e.Message);
            //            if (e.GetType() == typeof(ValidationException))
            //                return Results.Problem(e.Message, statusCode: 400);
            //            return Results.Problem(e.Message);
            //        }
            //    })
            //    .WithName("GetAreas")
            //    .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            //    .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            //    .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
        }
    }
}
