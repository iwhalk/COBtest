using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Data;
using Shared.Models;
using ReportesInmobiliaria.Interfaces;
using ReportesInmobiliaria.Repositories;
using ReportesInmobiliaria.Services;
using ReportesInmobiliaria.Utilities;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var secretKey = builder.Configuration.GetValue<string>("SecretKey");
var key = Encoding.ASCII.GetBytes(secretKey);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
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
builder.Services.AddScoped<CajeroReceptorRepository>();
builder.Services.AddScoped<TurnoCarrilesRepository>();
builder.Services.AddScoped<DiaCasetaRepository>();
builder.Services.AddScoped<MetodosGlbRepository>();
builder.Services.AddScoped<Validaciones>();
builder.Services.AddScoped<PdfFactory>();
builder.Services.AddScoped<IReportesService, ReportesService>();

builder.Services.AddCors();
//builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

//app.MapControllers();

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());

app.MapGet("/usuarioplaza", async (IReportesService _reportesService, ILogger<Program> _logger) =>
{
    try
    {
        var usuarioPlaza = await _reportesService.GetUsuarioPlazaAsync();
        return Results.Ok(usuarioPlaza);
    }
    catch (Exception e)
    {
        _logger.LogError(e, e.Message);
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("GetUsuarioPlaza")
.Produces<UsuarioPlaza>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
.AllowAnonymous();

app.MapGet("/delegaciones", async (IReportesService _reportesService, ILogger<Program> _logger) =>
{
    try
    {
        var delegaciones = await _reportesService.GetDelegacionesAsync();
        return Results.Ok(delegaciones);
    }
    catch (Exception e)
    {
        _logger.LogError(e, e.Message);
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("GetDelegaciones")
.Produces<List<TypeDelegacion>>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapGet("/plazas", async (IReportesService _reportesService, ILogger<Program> _logger) =>
{
    try
    {
        var plazas = await _reportesService.GetPlazasAsync();
        return Results.Ok(plazas);
    }
    catch (Exception e)
    {
        _logger.LogError(e, e.Message);
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("GetPlazas")
.Produces<List<TypePlaza>>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapGet("/turnos", (IReportesService _reportesService, ILogger<Program> _logger) =>
{
    try
    {
        var plazas = _reportesService.GetTurnos();
        return Results.Ok(plazas);
    }
    catch (Exception e)
    {
        _logger.LogError(e, e.Message);
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("GetTurnos")
.Produces<KeyValuePair<string, string>[]>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapGet("/administradores", (IReportesService _reportesService, ILogger<Program> _logger) =>
{
    try
    {
        var plazas = _reportesService.GetAdministradores();
        return Results.Ok(plazas);
    }
    catch (Exception e)
    {
        _logger.LogError(e, e.Message);
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("GetAdministradores")
.Produces<List<Personal>>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapGet("/encargadosturno", (IReportesService _reportesService, ILogger<Program> _logger) =>
{
    try
    {
        var encargados = _reportesService.GetEncargadosTurno();
        return Results.Ok(encargados);
    }
    catch (Exception e)
    {
        _logger.LogError(e, e.Message);
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("GetEncargadosTurno")
.Produces<List<Personal>>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapPost("/bolsascajeroreceptor", async (CajeroReceptor cajeroReceptor, IReportesService _reportesService, ILogger<Program> _logger) =>
{
    try
    {
        var bolsas = await _reportesService.GetBolsasCajeroReceptorAsync(cajeroReceptor);
        return Results.Ok(bolsas);
    }
    catch (Exception e)
    {
        _logger.LogError(e, e.Message);
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("GetBolsasCajeroReceptor")
.Produces<List<Bolsa>>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapPost("/reportecajeroreceptor", async (CajeroReceptor cajeroReceptor, IReportesService _reportesService, ILogger<Program> _logger) =>
{
    try
    {
        var reporte = await _reportesService.CreateReporteCajeroReceptorAsync(cajeroReceptor);
        return Results.File(reporte, "application/pdf");
    }
    catch (Exception e)
    {
        _logger.LogError(e, e.Message);
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("CreateReporteCajeroReceptor")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapPost("/reporteturnocarriles", async (TurnoCarriles turnoCarriles, IReportesService _reportesService, ILogger<Program> _logger) =>
{
    try
    {
        var reporte = await _reportesService.CreateReporteTurnoCarrilesAsync(turnoCarriles);
        return Results.File(reporte, "application/pdf");
    }
    catch (global::System.Exception e)
    {
        _logger.LogError(e, e.Message);
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("CreateReporteTurnoCarriles")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapPost("/reportediacaseta", async (DiaCaseta diaCaseta, IReportesService _reportesService, ILogger<Program> _logger) =>
{
    try
    {
        var reporte = await _reportesService.CreateReporteDiaCasetaAsync(diaCaseta);
        return Results.File(reporte, "application/pdf");
    }
    catch (Exception e)
    {
        _logger.LogError(e, e.Message);
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("CreateReporteDiaCaseta")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.Run();
