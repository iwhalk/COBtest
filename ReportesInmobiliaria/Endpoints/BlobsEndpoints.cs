using ReportesObra.Interfaces;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace ReportesObra.Endpoints
{
    public static class BlobsEndpoints
    {
        public static void MapBlobsEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/Blobs", async (int? id, IBlobService _blobService) =>
            {
            var blobs = await _blobService.GetBlobsAsync(id);
            if (blobs == null) return Results.NoContent();
            return Results.Ok(blobs);
            })
            .WithName("GetBlobs")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
            .AllowAnonymous();

            routes.MapGet("/Blob/{id}", async (int id, IBlobService _blobService) =>
            {
                var blob = await _blobService.GetBlobAsync(id);
                if (blob == null) return Results.NoContent();
                return Results.Ok(blob);
            })
            .WithName("GetBlob")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
            .AllowAnonymous();

            routes.MapGet("/BlobImage/{id}", async (int id, IBlobService _blobService) =>
            {
                var blobs = await _blobService.GetBlobFileAsync(id);
                if (blobs == null) return Results.NoContent();
                using MemoryStream ms = new MemoryStream();
                blobs.Content.CopyTo(ms);
                return Results.File(ms.ToArray(), blobs.ContentType);
            })
            .WithName("GetBlobFile")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
            .AllowAnonymous();

            routes.MapPost("/Blob", async (HttpRequest request, IBlobService _blobService) =>
            {
                try
                {
                    var file = request.Form.Files.FirstOrDefault();
                    if (file == null) return Results.BadRequest();
                    var res = await _blobService.CreateBlobAsync(file);
                    return Results.Ok(res);
                }
                catch (Exception e)
                {
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("CreateBlob")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
            .AllowAnonymous();

            routes.MapPut("/Blob/{id}", async (int id, SharedLibrary.Models.Blob blob, IBlobService _blobService) =>
            {
                try
                {
                    if (id != blob.IdBlob) return Results.BadRequest();
                    var res = await _blobService.UpdateBlobAsync(blob);
                    return Results.Ok(res);
                }
                catch (Exception e)
                {
                    if (e.GetType() == typeof(ValidationException))
                        return Results.Problem(e.Message, statusCode: 400);
                    return Results.Problem(e.Message);
                }
            })
            .WithName("UpdateBlob")
            .Produces<IResult>(StatusCodes.Status200OK)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
            .AllowAnonymous();

            //app.MapDelete("/Blob/{id}",
            //    async (int id, IBlobService _blobService) =>
            //    {
            //        try
            //        {
            //            var res = await _blobService.DeleteBlobAsync(id);
            //            if (res) return Results.NoContent();
            //            return Results.BadRequest();
            //        }
            //        catch (Exception e) 
            //        {
            //            if (e.GetType() == typeof(ValidationException))
            //                return Results.Problem(e.Message, statusCode: 400);
            //            return Results.Problem(e.Message);

            //        }
            //    })
            //.WithName("DeleteBlobAsync")
            //.Produces(StatusCodes.Status204NoContent)
            //.Produces(StatusCodes.Status400BadRequest)
            //.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            //.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            //app.MapGet("/BlobInventory", async (IBlobService _blobService) =>
            //{
            //    var blobs = await _blobService.GetBlobInventoryAsync();
            //    if (blobs == null) return Results.NoContent();
            //    return Results.Ok(blobs);
            //})
            //.WithName("GetBlobInventoryAsync")
            //.Produces<IResult>(StatusCodes.Status200OK)
            //.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            //.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            //app.MapPost("/BlobInventory", async (BlobsInventory blobsInventory, IBlobService _blobService) =>
            //{
            //    try
            //    {
            //        var res = await _blobService.CreateBlobInventoryAsync(blobsInventory);
            //        return Results.Ok(res);
            //    }
            //    catch (Exception e)
            //    {
            //        if (e.GetType() == typeof(ValidationException))
            //            return Results.Problem(e.Message, statusCode: 400);
            //        return Results.Problem(e.Message);
            //    }
            //})
            //.WithName("CreateBlobInventoryAsync")
            //.Produces<IResult>(StatusCodes.Status200OK)
            //.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            //.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            //app.MapPut("/BlobInventory/{id}", async (int id, BlobsInventory blobsInventory, IBlobService _blobService) =>
            //{
            //    try
            //    {
            //        if (id != blobsInventory.IdBlobsInventory) return Results.BadRequest();
            //        var res = await _blobService.UpdateBlobInventoryAsync(blobsInventory);
            //        return Results.Ok(res);
            //    }
            //    catch (Exception e)
            //    {
            //        if (e.GetType() == typeof(ValidationException))
            //            return Results.Problem(e.Message, statusCode: 400);
            //        return Results.Problem(e.Message);
            //    }
            //})
            //.WithName("UpdateBlobInventoryAsync")
            //.Produces<IResult>(StatusCodes.Status200OK)
            //.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            //.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

            //app.MapDelete("/BlobInventory/{id}",
            //    async (int id, IBlobService _blobService) =>
            //    {
            //        try
            //        {
            //            var res = await _blobService.DeleteBlobInventoryAsync(id);
            //            if (res) return Results.NoContent();
            //            return Results.BadRequest();
            //        }
            //        catch (Exception e)
            //        {
            //            if (e.GetType() == typeof(ValidationException))
            //                return Results.Problem(e.Message, statusCode: 400);
            //            return Results.Problem(e.Message);

            //        }
            //    })
            //.WithName("DeleteBlobInventoryAsync")
            //.Produces(StatusCodes.Status204NoContent)
            //.Produces(StatusCodes.Status400BadRequest)
            //.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            //.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
        }
    }
}
