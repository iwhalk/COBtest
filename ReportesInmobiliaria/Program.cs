using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Data;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using SixLabors.Fonts.Tables.AdvancedTypographic;
//using System.DirectoryServices.ActiveDirectory;
using Azure.Storage.Blobs;
using System.Configuration;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReportesObra.Interfaces;
using ReportesObra.Services;
using ReportesObra.Utilities;
using ReportesObra.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var connectionString = "Server=arisoft2245.database.windows.net;Database=prosisdb_4;User=PROSIS_DEVELOPER;Password=PR0515_D3ev3l0p3r;MultipleActiveResultSets=true";
var secretKey = builder.Configuration.GetValue<string>("SecretKey");
var key = Encoding.ASCII.GetBytes(secretKey);

CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

Thread.CurrentThread.CurrentCulture = culture;
Thread.CurrentThread.CurrentUICulture = culture;

builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetConnectionString("AzureStorage")));

// Add services to the container.
builder.Services.AddDbContext<ObraDbContext>(options =>
    options.UseSqlServer(connectionString).EnableSensitiveDataLogging());
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthentication();
//builder.Services.AddAuthorization(cfg =>
//{
//    cfg.FallbackPolicy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//});

//builder.Services.AddLogging(loggingBuilder =>
//{
//    var loggingSection = builder.Configuration.GetSection("Logging");
//    loggingBuilder.AddFile(loggingSection);
//});
//builder.Services.AddScoped<CreationLogger>();

builder.Services.AddScoped<AuxiliaryMethods>();
//builder.Services.AddScoped<IAspNetUserService, AspNetUserService>();
builder.Services.AddScoped<IApartmentsService, ApartmentsService>();
builder.Services.AddScoped<IAreasService, AreasService>();
builder.Services.AddScoped<IBuildingsService, BuildingsService>();
builder.Services.AddScoped<IActivitiesService, ActivitiesService>();
builder.Services.AddScoped<IElementsService, ElementsService>();
builder.Services.AddScoped<ISubElementsService, SubElementsService>();
builder.Services.AddScoped<IReporteDetallesService, ReporteDetallesService>();

builder.Services.AddScoped<ReportesFactory>();

builder.Services.AddCors();
//builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();

//app.UseAuthorization();

//app.UseHttpsRedirection();

//app.MapControllers();

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());
app.MapApartmentEndpoints();
app.MapAreaEndpoints();
app.MapBuildingEndpoints();
app.MapActivityEndpoints();
app.MapElementEndpoints();
app.MapSubElementEndpoints();
app.MapReporteEndpoints();

#region Inmobiliaria

#region AspNetUsers
//app.MapGet("/AspNetUsers", async (IAspNetUserService _aspNetUserService, ILogger<Program> _logger) =>
//{
//    try
//    {
//        var aspNetUsers = await _aspNetUserService.GetAspNetUsersAsync();
//        if (aspNetUsers == null) return Results.NoContent();
//        return Results.Ok(aspNetUsers);
//    }
//    catch (Exception e)
//    {
//        _logger.LogError(e, e.Message);
//        if (e.GetType() == typeof(ValidationException))
//            return Results.Problem(e.Message, statusCode: 400);
//        return Results.Problem(e.Message);

//    }
//})
//.WithName("GetAspNetUsers")
//.Produces<IResult>(StatusCodes.Status200OK)
//.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
//.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
#endregion

# region Blob

//app.MapGet("/Blobs", async (int id, IBlobService _blobService) =>
//{
//    var blobs = await _blobService.GetBlobAsync(id);
//    if (blobs == null) return Results.NoContent();
//    using MemoryStream ms = new MemoryStream();
//    blobs.Content.CopyTo(ms);
//    return Results.File(ms.ToArray(), blobs.ContentType);
//})
//.WithName("GetBlobAsync")
//.Produces<IResult>(StatusCodes.Status200OK)
//.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
//.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
//.AllowAnonymous();

//app.MapPost("/Blobs", async (string name, HttpRequest request, IBlobService _blobService) =>
//{
//    try
//    {
//        var file = request.Form.Files.FirstOrDefault();
//        if(file == null) return Results.BadRequest();
//        var res = await _blobService.CreateBlobAsync(name, file);
//        return Results.Ok(res);
//    }
//    catch (Exception e)
//    {
//        if (e.GetType() == typeof(ValidationException))
//            return Results.Problem(e.Message, statusCode: 400);
//        return Results.Problem(e.Message);
//    }
//})
//.WithName("CreateBlobAsync")
//.Produces<IResult>(StatusCodes.Status200OK)
//.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
//.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
//.AllowAnonymous();

//app.MapPut("/Blobs/{id}", async (int id, SharedLibrary.Models.Blob blob, IBlobService _blobService) =>
//{
//    try
//    {
//        if (id != blob.IdBlobs) return Results.BadRequest();
//        var res = await _blobService.UpdateBlobAsync(blob);
//        return Results.Ok(res);
//    }
//    catch (Exception e)
//    {
//        if (e.GetType() == typeof(ValidationException))
//            return Results.Problem(e.Message, statusCode: 400);
//        return Results.Problem(e.Message);
//    }
//})
//.WithName("UpdateBlobAsync")
//.Produces<IResult>(StatusCodes.Status200OK)
//.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
//.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

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

#endregion

static string? GetNullableString(string? value) => !string.IsNullOrWhiteSpace(value) && value.ToUpper().Contains("NULL") ? null : value;
#endregion
app.Run();
