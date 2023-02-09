using ReportesObra.Interfaces;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace ReportesObra.Endpoints
{
	public static class ImageEndpoints
	{
		public static void MapImageEndpoints(this IEndpointRouteBuilder routes)
		{
			routes.MapPost("/MixImages", async (ImageData imageData, IImageService _imageService) =>
			{
				try
				{
					var res = await _imageService.MixImage(imageData);
					return Results.Ok(res);
				}
				catch (Exception e)
				{
					if (e.GetType() == typeof(ValidationException))
						return Results.Problem(e.Message, statusCode: 400);
					return Results.Problem(e.Message);
				}
			})
			.WithName("MixImage")
			.Produces<IResult>(StatusCodes.Status200OK)
			.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
			.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
			.AllowAnonymous();

		}
	}
}
