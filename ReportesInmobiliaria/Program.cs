using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Data;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ReportesInmobiliaria.Interfaces;
using ReportesInmobiliaria.Services;
using ReportesInmobiliaria.Utilities;
using Shared.Data;
using Shared.Models;


var builder = WebApplication.CreateBuilder(args);
var connectionString = "Server=prosisdev.database.windows.net;Database=prosisdb_3;User=PROSIS_DEVELOPER;Password=PR0515_D3ev3l0p3r;MultipleActiveResultSets=true";
var secretKey = builder.Configuration.GetValue<string>("SecretKey");
var key = Encoding.ASCII.GetBytes(secretKey);

CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

Thread.CurrentThread.CurrentCulture = culture;
Thread.CurrentThread.CurrentUICulture = culture;

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

builder.Services.AddAuthentication();
builder.Services.AddAuthorization(cfg =>
{
    cfg.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

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
builder.Services.AddScoped<IReportesService, ReportesService>();
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
#region Inmobiliaria

#region Lessor
app.MapGet("/Lessor",
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

app.MapPost("/Lessor", async (Shared.Models.Lessor lessor, ILessorService _lessorService) =>
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
app.MapGet("/Tenant",
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

app.MapPost("/Tenant", async (Shared.Models.Tenant tenant, ITenantService _tenantService) =>
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
app.MapGet("/Inventory",
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

app.MapPost("/Inventory", async (Shared.Models.Inventory inventory, IInventoryService _inventoryService) =>
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

app.MapPost("/Services", async (Shared.Models.Service service, IServicesService _servicesService) =>
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

app.MapPost("/Descriptions", async (Shared.Models.Description description, IDescriptionService _descriptionService) =>
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

#region Porperty
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

app.MapPost("/PropertyTypes", async (PropertyType propertyType, IPropertyTypesService _propertyTypesService) =>
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

app.MapPut("/PropertyTypes", async (PropertyType propertyType, IPropertyTypesService _propertyTypesService) =>
{
    try
    {
        var res = await _propertyTypesService.UpdatePropertyTypeAsync(propertyType);
        return Results.Ok(res);
    }
    catch (Exception e)
    {
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("UpdatePropertyType")
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

app.MapPost("/Properties", async (Property property, IPropertiesService _propertiesService) =>
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

app.MapPut("/Properties", async (Property property, IPropertiesService _propertiesService) =>
{
    try
    {
        var res = await _propertiesService.UpdatePropertyAsync(property);
        return Results.Ok(res);
    }
    catch (Exception e)
    {
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("UpdateProperty")
.Produces<IResult>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
#endregion

#region ReportesPDF
app.MapGet("/ReporteArrendores", async (int? id,IReportesService _reportesService,ILogger<Program> _logger) =>
{
    try

    {
        var newModule = await _reportesService.GetReporteArrendadores(id);
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
.WithName("GetReporteArrendadores")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
//.AllowAnonymous();

//app.MapGet("/ReporteFeatures", async (int? id, IReportesService _reportesService, ILogger<Program> _logger) =>
//{
//    try

//    {
//        var newModule = await _reportesService.GetReporteArrendadores(id);
//        if (newModule == null) return Results.NoContent();
//        //System.IO.File.WriteAllBytes("ReporteTransaccionesCrucesTotales.pdf", newModule);
//        return Results.File(newModule, "application/pdf");
//    }
//    catch (Exception e)
//    {
//        _logger.LogError(e, e.Message);
//        if (e.GetType() == typeof(ValidationException))
//            return Results.Problem(e.Message, statusCode: 400);
//        return Results.Problem(e.Message);
//    }
//})
//.WithName("GetReporteArrendadores")
//.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
//.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
//.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

#endregion

#endregion
app.Run();
