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
var connectionString = "Server=soft2245cob.database.windows.net;Database=prosisdb_4;User=PROSIS_DEVELOPER;Password=PR0515_D3ev3l0p3r;MultipleActiveResultSets=true";
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
builder.Services.AddAuthorization(cfg =>
{
    cfg.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

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
builder.Services.AddScoped<IProgressReportsService, ProgressReportsService>();
builder.Services.AddScoped<IProgressLogsService, ProgressLogsService>();
builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.AddScoped<IElementsService, ElementsService>();
builder.Services.AddScoped<ISubElementsService, SubElementsService>();
builder.Services.AddScoped<IReportesService, ReportsService>();

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

//app.UseAuthentication();

app.UseAuthorization();

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
app.MapProgressLogsEndpoints();
app.MapProgressReportsEndpoints();
app.MapBlobsEndpoints();
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

static string? GetNullableString(string? value) => !string.IsNullOrWhiteSpace(value) && value.ToUpper().Contains("NULL") ? null : value;
#endregion
app.Run();
