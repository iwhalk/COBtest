using ReportesObra.Interfaces;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace ReportesObra.Endpoints
{
    public static class ApartmentEndpoints
    {
        public static void MapApartmentEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/Apartments", async (IApartmentsService _apartmentsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var apartments = await _apartmentsService.GetApartmentsAsync();
                    return Results.Ok(apartments);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetApartments")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapGet("/Apartment", async (int idApartment, IApartmentsService _apartmentsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var apartment = await _apartmentsService.GetApartmentAsync(idApartment);
                    return Results.Ok(apartment);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("GetApartment")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPost("/Apartment", async (Apartment apartment, IApartmentsService _apartmentsService, ILogger<Program> _logger) =>
            {
                try
                {
                    var res = await _apartmentsService.CreateApartmentAsync(apartment);
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
            .WithName("CreateApartment")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            routes.MapPut("/Apartment/{id}", async (int id, Apartment apartment, IApartmentsService _apartmentsService, ILogger<Program> _logger) =>
            {
                try
                {
                    if (id != apartment.IdApartment) return Results.BadRequest();
                    var res = await _apartmentsService.UpdateApartmentAsync(apartment);
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
            .WithName("UpdateApartment")
            .Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
        }
    }
}
