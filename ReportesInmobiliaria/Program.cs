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
using ReportesInmobiliaria.Interfaces;
using ReportesInmobiliaria.Services;
using ReportesInmobiliaria.Utilities;
using Microsoft.AspNetCore.Http;
using SixLabors.Fonts.Tables.AdvancedTypographic;
using SharedLibrary.Models;
using System.DirectoryServices.ActiveDirectory;
using Azure.Storage.Blobs;
using System.Configuration;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


var builder = WebApplication.CreateBuilder(args);
var connectionString = "Server=prosisdev.database.windows.net;Database=prosisdb_3;User=PROSIS_DEVELOPER;Password=PR0515_D3ev3l0p3r;MultipleActiveResultSets=true";
var secretKey = builder.Configuration.GetValue<string>("SecretKey");
var key = Encoding.ASCII.GetBytes(secretKey);

CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

Thread.CurrentThread.CurrentCulture = culture;
Thread.CurrentThread.CurrentUICulture = culture;

builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetConnectionString("AzureStorage")));

// Add services to the container.
builder.Services.AddDbContext<InmobiliariaDbContext>(options =>
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

//builder.Services.AddAuthentication();
//builder.Services.AddAuthorization(cfg =>
//{
//    cfg.FallbackPolicy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//});

builder.Services.AddLogging(loggingBuilder =>
{
    var loggingSection = builder.Configuration.GetSection("Logging");
    loggingBuilder.AddFile(loggingSection);
});
//builder.Services.AddScoped<CreationLogger>();

//builder.Services.AddScoped<IModulesService, ModulesService>();
//builder.Services.AddScoped<ITagsService, TagsService>();
//builder.Services.AddScoped<ITelepeajeService, TelepeajeService>();
//builder.Services.AddScoped<IReportesService, ReportesService>();
builder.Services.AddScoped<IPropertyTypesService, PropertyTypesService>();
builder.Services.AddScoped<IPropertiesService, PropertiesService>();
builder.Services.AddScoped<ILessorService, LessorService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IServicesService, ServicesService>();
builder.Services.AddScoped<IDescriptionService, DescriptionService>();
builder.Services.AddScoped<IAreasService, AreasService>();
builder.Services.AddScoped<IFeaturesService, FeaturesService>();
//builder.Services.AddScoped<IReportesService, ReportesService>();
//builder.Services.AddScoped<IReporteFeaturesService, ReporteFeaturesService>();
builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.AddScoped<IReceptionCertificates, ReceptionCertificatesService>();
builder.Services.AddScoped<AuxiliaryMethods>();
builder.Services.AddScoped<IReporteActaEntregaService, ReporteActaEntregaService>();
builder.Services.AddScoped<IInmobiliariaDbContextProcedures, InmobiliariaDbContextProcedures>();
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

app.UseAuthorization();

//app.UseHttpsRedirection();

//app.MapControllers();

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());
#region Inmobiliaria

#region Lessor
app.MapGet("/Lessors",
    async (ILessorService _lessorService, ILogger<Program> _logger) =>
    {
        try
        {
            var lessors = await _lessorService.GetLessorsAsync();
            return Results.Ok(lessors);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            if (e.GetType() == typeof(ValidationException))
                return Results.Problem(e.Message, statusCode: 400);
            return Results.Problem(e.Message);

        }
    })
.WithName("GetLessors")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapPost("/Lessor", async (Lessor lessor, ILessorService _lessorService) =>
{
    try
    {
        var res = await _lessorService.CreateLessorAsync(lessor);
        return Results.Ok(res);
    }
    catch (Exception e)
    {
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("CreateLessor")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

#endregion

#region Tenant
app.MapGet("/Tenants",
    async (ITenantService _tenantService , ILogger<Program> _logger) =>
    {
        try
        {
            var tenants = await _tenantService.GetTenantAsync();
            return Results.Ok(tenants);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            if (e.GetType() == typeof(ValidationException))
                return Results.Problem(e.Message, statusCode: 400);
            return Results.Problem(e.Message);

        }
    })
.WithName("GetTenant")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapPost("/Tenant", async (Tenant tenant, ITenantService _tenantService) =>
{
    try
    {
        var res = await _tenantService.CreateTenantAsync(tenant);
        return Results.Ok(res);
    }
    catch (Exception e)
    {
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("CreateTenantAsync")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
#endregion

#region Inventory
app.MapGet("/Inventories",
    async (IInventoryService _inventoryService, ILogger<Program> _logger) =>
    {
        try
        {
            var inventories = await _inventoryService.GetInventoryAsync();
            return Results.Ok(inventories);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            if (e.GetType() == typeof(ValidationException))
                return Results.Problem(e.Message, statusCode: 400);
            return Results.Problem(e.Message);

        }
    })
.WithName("GetInventoryAsync")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapPost("/Inventory", async (SharedLibrary.Models.Inventory inventory, IInventoryService _inventoryService) =>
{
    try
    {
        var res = await _inventoryService.CreateInventoryAsync(inventory);
        return Results.Ok(res);
    }
    catch (Exception e)
    {
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("CreateInventoryAsync")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
#endregion

#region Services
app.MapGet("/Services",
    async (IServicesService _servicesService, ILogger<Program> _logger) =>
    {
        try
        {
            var inventories = await _servicesService.GetServicesAsync();
            return Results.Ok(inventories);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            if (e.GetType() == typeof(ValidationException))
                return Results.Problem(e.Message, statusCode: 400);
            return Results.Problem(e.Message);

        }
    })
.WithName("GetServicesAsync")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapPost("/Services", async (SharedLibrary.Models.Service service, IServicesService _servicesService) =>
{
    try
    {
        var res = await _servicesService.CreateServicesAsync(service);
        return Results.Ok(res);
    }
    catch (Exception e)
    {
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("CreateServicesAsync")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
#endregion

#region Descriptions
app.MapGet("/Descriptions",
    async (IDescriptionService _descriptionService, ILogger<Program> _logger) =>
    {
        try
        {
            var descriptions = await _descriptionService.GetDescriptionAsync();
            return Results.Ok(descriptions);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            if (e.GetType() == typeof(ValidationException))
                return Results.Problem(e.Message, statusCode: 400);
            return Results.Problem(e.Message);

        }
    })
.WithName("GetDescriptionAsync")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapPost("/Description", async (SharedLibrary.Models.Description description, IDescriptionService _descriptionService) =>
{
    try
    {
        var res = await _descriptionService.CreateDescriptionAsync(description);
        return Results.Ok(res);
    }
    catch (Exception e)
    {
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("CreateDescriptionAsync")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

#endregion

#region PropertyType
app.MapGet("/PropertyTypes", async (IPropertyTypesService _propertyTypesService) =>
{
    var propertyTypes = await _propertyTypesService.GetPropertyTypesAsync();
    if (propertyTypes == null) return Results.NoContent();
    return Results.Ok(propertyTypes);
})
.WithName("GetPropertyTypes")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapPost("/PropertyType", async (PropertyType propertyType, IPropertyTypesService _propertyTypesService) =>
{
    try
    {
        var res = await _propertyTypesService.CreatePropertyTypeAsync(propertyType);
        return Results.Ok(res);
    }
    catch (Exception e)
    {
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("CreatePropertyType")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
#endregion

#region Properties
app.MapGet("/Properties", async (IPropertiesService _propertiesService) =>
{
    var propertyTypes = await _propertiesService.GetPropertiesAsync();
    if (propertyTypes == null) return Results.NoContent();
    return Results.Ok(propertyTypes);
})
.WithName("GetProperties")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapPost("/Property", async (SharedLibrary.Models.Property property, IPropertiesService _propertiesService) =>
{
    try
    {
        var res = await _propertiesService.CreatePropertyAsync(property);
        return Results.Ok(res);
    }
    catch (Exception e)
    {
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("CreateProperty")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
#endregion

#region Areas
app.MapGet("/Areas", async (IAreasService _areasService, ILogger<Program> _logger) =>
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

app.MapPost("/Area", async (Area area, IAreasService _areasService, ILogger<Program> _logger) =>
{
    try
    {
        var res = await _areasService.CreateAreaAsync(area);
        return Results.Ok(res);
    }
    catch (Exception e)
    {
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("CreateArea")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
#endregion Areas

#region Features
app.MapGet("/Features", async (IFeaturesService _featuresService, ILogger<Program> _logger) =>
{
    try
    {
        var features = await _featuresService.GetFeaturesAsync();
        return Results.Ok(features);
    }
    catch (Exception e)
    {
        _logger.LogError(e, e.Message);
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }

})
.WithName("GetFeatures")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapPost("/Feature", async (Feature feature, IFeaturesService _featuresService, ILogger<Program> _logger) =>
{
    try
    {
        var res = await _featuresService.CreateFeatureAsync(feature);
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
.WithName("CreateFeature")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
#endregion Features

#region ReportesPDF
app.MapGet("/ReporteActaEntrega", async (int idProperty, IReporteActaEntregaService _reportesService, ILogger<Program> _logger) =>
{
    try

    {
        var newModule = await _reportesService.GetActaEntrega(idProperty);
        if (newModule == null) return Results.NoContent();
        //System.IO.File.WriteAllBytes("ReporteTransaccionesCrucesTotales.pdf", newModule);
        return Results.File(newModule, "application/pdf");
    }
    catch (Exception e)
    {
        _logger.LogError(e, e.Message);
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("GetReporteActaEntrega")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

#endregion

# region Blob

app.MapGet("/Blobs", async (int id, IBlobService _blobService) =>
{
    var blobs = await _blobService.GetBlobAsync(id);
    if (blobs == null) return Results.NoContent();
    using MemoryStream ms = new MemoryStream();
    blobs.Content.CopyTo(ms);
    return Results.File(ms.ToArray(), blobs.ContentType);
})
.WithName("GetBlobAsync")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
.AllowAnonymous();

app.MapPost("/Blobs", async (string name, [FromForm(Name = "file")] HttpRequest request, IBlobService _blobService) =>
{
    try
    {
        var file = request.Form.Files;
        var res = await _blobService.CreateBlobAsync(name, file);
        return Results.Ok(res);
    }
    catch (Exception e)
    {
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("CreateBlobAsync")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
.AllowAnonymous();

app.MapPut("/Blobs/{id}", async (int id, SharedLibrary.Models.Blob blob, IBlobService _blobService) =>
{
    try
    {
        if (id != blob.IdBlobs) return Results.BadRequest();
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
.WithName("UpdateBlobAsync")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapDelete("/Blob/{id}",
    async (int id, IBlobService _blobService) =>
    {
        try
        {
            var res = await _blobService.DeleteBlobAsync(id);
            if (res) return Results.NoContent();
            return Results.BadRequest();
        }
        catch (Exception e) 
        {
            if (e.GetType() == typeof(ValidationException))
                return Results.Problem(e.Message, statusCode: 400);
            return Results.Problem(e.Message);

        }
    })
.WithName("DeleteBlobAsync")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status400BadRequest)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapGet("/BlobInventory", async (IBlobService _blobService) =>
{
    var blobs = await _blobService.GetBlobInventoryAsync();
    if (blobs == null) return Results.NoContent();
    return Results.Ok(blobs);
})
.WithName("GetBlobInventoryAsync")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapPost("/BlobInventory", async (BlobsInventory blobsInventory, IBlobService _blobService) =>
{
    try
    {
        var res = await _blobService.CreateBlobInventoryAsync(blobsInventory);
        return Results.Ok(res);
    }
    catch (Exception e)
    {
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("CreateBlobInventoryAsync")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapPut("/BlobInventory/{id}", async (int id, BlobsInventory blobsInventory, IBlobService _blobService) =>
{
    try
    {
        if (id != blobsInventory.IdBlobsInventory) return Results.BadRequest();
        var res = await _blobService.UpdateBlobInventoryAsync(blobsInventory);
        return Results.Ok(res);
    }
    catch (Exception e)
    {
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("UpdateBlobInventoryAsync")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapDelete("/BlobInventory/{id}",
    async (int id, IBlobService _blobService) =>
    {
        try
        {
            var res = await _blobService.DeleteBlobInventoryAsync(id);
            if (res) return Results.NoContent();
            return Results.BadRequest();
        }
        catch (Exception e)
        {
            if (e.GetType() == typeof(ValidationException))
                return Results.Problem(e.Message, statusCode: 400);
            return Results.Problem(e.Message);

        }
    })
.WithName("DeleteBlobInventoryAsync")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status400BadRequest)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

#endregion

#region ReceptionCertificates

app.MapGet("/ReceptionCertificate", async (string? startDay, string? endDay, int? certificateType, int? propertyType, int? numberOfRooms, int? lessor, int? tenant, string? delegation, string? agent, int? currentPage, int? rowNumber, IReceptionCertificates _receptionCertificates,AuxiliaryMethods _auxiliaryMethods , ILogger<Program> _logger) =>
{
    try
    {
        startDay = GetNullableString(startDay);
        endDay = GetNullableString(endDay);

        string patternDia = @"(19|20)\d\d[-/.](0[1-9]|1[012])[-/.](0[1-9]|[1][0-9]|[2][0-9]|3[01])";

        if (startDay != null)
        {
            if (Regex.IsMatch(startDay, patternDia) == false)
            {
                return Results.Problem("El dia se encuentra en un formato incorrecto", statusCode: 400);
            }
        }
        if (endDay != null)
        {
            if (Regex.IsMatch(endDay, patternDia) == false)
            {
                return Results.Problem("El dia se encuentra en un formato incorrecto", statusCode: 400);
            }
        }
        var dates = new Dates();

        if (startDay != null && endDay != null)
        {
            TimeSpan ts = new TimeSpan(00, 00, 0);
            dates = new Dates()
            {
                StartDate = DateTime.Parse(startDay).Date + ts,
                EndDate = DateTime.Parse(endDay).Date.AddHours(23).AddMinutes(59)
            };
        }
        else
            dates = null;

        var actas = await _receptionCertificates.GetReceptionCertificatesAsync(dates, certificateType, propertyType, numberOfRooms, lessor, tenant, delegation, agent);
        if (currentPage != null && rowNumber != null)
        {
            actas = actas.Skip((int)((currentPage - 1) * rowNumber)).Take((int)rowNumber).ToList();
        }
            return Results.Ok(actas);
    }
    catch (Exception e)
    {
        _logger.LogError(e, e.Message);
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }

})
.WithName("GetReceptionCertificatesAsync")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapPost("/ReceptionCertificate", async (ReceptionCertificate receptionCertificate, IReceptionCertificates _receptionCertificates, ILogger<Program> _logger) =>
{
    try
    {
        var res = await _receptionCertificates.CreateReceptionCertificateAsync(receptionCertificate);
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
.WithName("CreateReceptionCertificateAsync")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");



app.MapPut("/ReceptionCertificate", async (ReceptionCertificate receptionCertificate, IReceptionCertificates _receptionCertificates, ILogger<Program> _logger) =>
{
    try
    {
        var res = await _receptionCertificates.UpdateReceptionCertificateAsync(receptionCertificate);
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
.WithName("EditReceptionCertificateAsync")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

#endregion
static string? GetNullableString(string? value) => !string.IsNullOrWhiteSpace(value) && value.ToUpper().Contains("NULL") ? null : value;
#endregion
app.Run();
