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

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = "Server=10.1.1.135;Database=BackOfficeIntermodalCapa;User=PROSIS_DEV;Password=Pr0$1$D3v;MultipleActiveResultSets=true";
var secretKey = builder.Configuration.GetValue<string>("SecretKey");
var key = Encoding.ASCII.GetBytes(secretKey);

CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

Thread.CurrentThread.CurrentCulture = culture;
Thread.CurrentThread.CurrentUICulture = culture;

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
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

builder.Services.AddScoped<IModulesService, ModulesService>();
builder.Services.AddScoped<ITagsService, TagsService>();
builder.Services.AddScoped<ITelepeajeService, TelepeajeService>();
builder.Services.AddScoped<IReportesService, ReportesService>();
builder.Services.AddScoped<ReportesFactory>();
builder.Services.AddScoped<AuxiliaryMethods>();

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
#region Reportes

app.MapGet("/Reportes/TransaccionesCrucesTotales", async (string? dia, string? mes, string? semana, string? tag, string ? noDePlaca, string ? noEconomico, IReportesService _reportesService, ILogger<Program> _logger) =>
{
    try
    
    {
        dia = GetNullableString(dia);
        mes = GetNullableString(mes);
        semana = GetNullableString(semana);

        string patternDia = @"(19|20)\d\d[-/.](0[1-9]|1[012])[-/.](0[1-9]|[1][0-9]|[2][0-9]|3[01])";
            
        if (dia == null && mes == null && semana == null)
            return Results.Problem("No se envio ninguna fecha", statusCode: 400);

        if (dia != null)
        {
            if (Regex.IsMatch(dia, patternDia) == false)
            {
                return Results.Problem("El dia se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        string patternMes = @"(19|20)\d\d[-/.](0[1-9]|1[012])";

        if (mes != null)
        {
            if (Regex.IsMatch(mes, patternMes) == false)
            {
                return Results.Problem("El mes se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        string patternSemana = @"(19|20)\d\d[-/.](W0[1-9]|W1[0-9]|W2[0-9]|W3[0-9]|W4[0-9]|W5[0-3])";

        if (semana != null)
        {
            if (Regex.IsMatch(semana, patternSemana) == false)
            {
                return Results.Problem("La semana se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        if ((dia != null && semana != null) || (dia != null && mes != null) || (mes != null && semana != null))
            return Results.Problem("Mas de una fecha ingresada", statusCode: 400);

        var newModule = await _reportesService.GetReporteTransaccionesCrucesTotales(dia, mes, semana, tag, noDePlaca, noEconomico);
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
.WithName("ReporteTransaccionesCrucesTotales")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
//.AllowAnonymous();

app.MapGet("/Reportes/ReporteCrucesFerromex", async (string? dia, string? mes, string? semana, string? tag, string? noDePlaca, string? noEconomico, IReportesService _reportesService, ILogger<Program> _logger) =>
{
    try
    {
        dia = GetNullableString(dia);
        mes = GetNullableString(mes);
        semana = GetNullableString(semana);

        string patternDia = @"(19|20)\d\d[-/.](0[1-9]|1[012])[-/.](0[1-9]|[1][0-9]|[2][0-9]|3[01])";

        if (dia == null && mes == null && semana == null)
            return Results.Problem("No se envio ninguna fecha", statusCode: 400);

        if (dia != null)
        {
            if (Regex.IsMatch(dia, patternDia) == false)
            {
                return Results.Problem("El dia se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        string patternMes = @"(19|20)\d\d[-/.](0[1-9]|1[012])";

        if (mes != null)
        {
            if (Regex.IsMatch(mes, patternMes) == false)
            {
                return Results.Problem("El mes se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        string patternSemana = @"(19|20)\d\d[-/.](W0[1-9]|W1[0-9]|W2[0-9]|W3[0-9]|W4[0-9]|W5[0-3])";

        if (semana != null)
        {
            if (Regex.IsMatch(semana, patternSemana) == false)
            {
                return Results.Problem("La semana se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        if ((dia != null && semana != null) || (dia != null && mes != null) || (mes != null && semana != null))
            return Results.Problem("Mas de una fecha ingresada", statusCode: 400);

        var newModule = await _reportesService.GetReporteCrucesFerromex(dia, mes, semana, tag, noDePlaca, noEconomico);
        if (newModule == null) return Results.NoContent();
        //System.IO.File.WriteAllBytes("ReporteCrucesFerromex.pdf", newModule);
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
.WithName("ReporteCrucesFerromex")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
//.AllowAnonymous();

app.MapGet("/Reportes/ReporteCrucesFerromexResumen", async (string? dia, string? mes, string? semana, string? tag, string? noDePlaca, string? noEconomico, IReportesService _reportesService, ILogger<Program> _logger) =>
{
    try
    {
        dia = GetNullableString(dia);
        mes = GetNullableString(mes);
        semana = GetNullableString(semana);

        string patternDia = @"(19|20)\d\d[-/.](0[1-9]|1[012])[-/.](0[1-9]|[1][0-9]|[2][0-9]|3[01])";

        if (dia == null && mes == null && semana == null)
            return Results.Problem("No se envio ninguna fecha", statusCode: 400);

        if (dia != null)
        {
            if (Regex.IsMatch(dia, patternDia) == false)
            {
                return Results.Problem("El dia se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        string patternMes = @"(19|20)\d\d[-/.](0[1-9]|1[012])";

        if (mes != null)
        {
            if (Regex.IsMatch(mes, patternMes) == false)
            {
                return Results.Problem("El mes se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        string patternSemana = @"(19|20)\d\d[-/.](W0[1-9]|W1[0-9]|W2[0-9]|W3[0-9]|W4[0-9]|W5[0-3])";

        if (semana != null)
        {
            if (Regex.IsMatch(semana, patternSemana) == false)
            {
                return Results.Problem("La semana se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        if ((dia != null && semana != null) || (dia != null && mes != null) || (mes != null && semana != null))
            return Results.Problem("Mas de una fecha ingresada", statusCode: 400);

        var newModule = await _reportesService.GetReporteCrucesFerromexResumen(dia, mes, semana, tag, noDePlaca, noEconomico);
        if (newModule == null) return Results.NoContent();
        //System.IO.File.WriteAllBytes("ReporteCrucesFerromexResumen.pdf", newModule);
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
.WithName("ReporteCrucesFerromexResumen")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
//.AllowAnonymous();

app.MapGet("/Reportes/ReporteIngresosResumen", async (string? dia, string? mes, string? semana, IReportesService _reportesService, ILogger<Program> _logger) =>
{
    try
    {
        dia = GetNullableString(dia);
        mes = GetNullableString(mes);
        semana = GetNullableString(semana);

        string patternDia = @"(19|20)\d\d[-/.](0[1-9]|1[012])[-/.](0[1-9]|[1][0-9]|[2][0-9]|3[01])";

        if (dia == null && mes == null && semana == null)
            return Results.Problem("No se envio ninguna fecha", statusCode: 400);

        if (dia != null)
        {
            if (Regex.IsMatch(dia, patternDia) == false)
            {
                return Results.Problem("El dia se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        string patternMes = @"(19|20)\d\d[-/.](0[1-9]|1[012])";

        if (mes != null)
        {
            if (Regex.IsMatch(mes, patternMes) == false)
            {
                return Results.Problem("El mes se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        string patternSemana = @"(19|20)\d\d[-/.](W0[1-9]|W1[0-9]|W2[0-9]|W3[0-9]|W4[0-9]|W5[0-3])";

        if (semana != null)
        {
            if (Regex.IsMatch(semana, patternSemana) == false)
            {
                return Results.Problem("La semana se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        if ((dia != null && semana != null) || (dia != null && mes != null) || (mes != null && semana != null))
            return Results.Problem("Mas de una fecha ingresada", statusCode: 400);

        var newModule = await _reportesService.GetReporteIngresosResumen(dia, mes, semana);
        if (newModule == null) return Results.NoContent();
        //System.IO.File.WriteAllBytes("ReporteIngresosResumen.pdf", newModule);
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
.WithName("GetReporteIngresosResumen")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
//.AllowAnonymous();

app.MapGet("/Reportes/TransaccionesTurno", async (string? carril, string? fecha, IReportesService _reportesService, ILogger<Program> _logger) =>
{
    try
    {
        fecha = GetNullableString(fecha);

        string patternDia = @"(19|20)\d\d[-/.](0[1-9]|1[012])[-/.](0[1-9]|[1][0-9]|[2][0-9]|3[01])";

        if (Regex.IsMatch(fecha, patternDia) == false)
        {
            return Results.Problem("La fecha se encuentra en un formato incorrecto", statusCode: 400);
        }

        var newModule = await _reportesService.GetReporteTransaccionesTurno(carril, fecha);
        if (newModule == null) return Results.NoContent();
        //System.IO.File.WriteAllBytes("TransaccionesTurno.pdf", newModule);
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
.WithName("GetReporteTransaccionesTurno")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapGet("/Reportes/ConcentradoTurno", async (string? carril, string? fecha, IReportesService _reportesService, ILogger<Program> _logger) =>
{
    try
    {
        fecha = GetNullableString(fecha);

        string patternDia = @"(19|20)\d\d[-/.](0[1-9]|1[012])[-/.](0[1-9]|[1][0-9]|[2][0-9]|3[01])";

        if (Regex.IsMatch(fecha, patternDia) == false)
        {
            return Results.Problem("La fecha se encuentra en un formato incorrecto", statusCode: 400);
        }

        var newModule = await _reportesService.GetReporteConcentradoTurno(carril, fecha);
        if (newModule == null) return Results.NoContent();
        //System.IO.File.WriteAllBytes("ReporteConcentradoTurno.pdf", newModule);
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
.WithName("GetReporteConcentradoTurno")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
//.AllowAnonymous();

app.MapGet("/Reportes/MantenimientoTags", async (string? tag, bool? estatus, string? fecha, string? noDePlaca, string? noEconomico, IReportesService _reportesService, ILogger<Program> _logger) =>
{
    try
    {
        fecha = GetNullableString(fecha);

        string patternDia = @"(19|20)\d\d[-/.](0[1-9]|1[012])[-/.](0[1-9]|[1][0-9]|[2][0-9]|3[01])";

        if (fecha != null && Regex.IsMatch(fecha, patternDia) == false)
        {
            return Results.Problem("La fecha se encuentra en un formato incorrecto", statusCode: 400);
        }

        var newModule = await _reportesService.GetReporteMantenimientoTags(tag, estatus, fecha, noDePlaca, noEconomico);
        if (newModule == null) return Results.NoContent();
        //System.IO.File.WriteAllBytes("ReporteTags.pdf", newModule);
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
.WithName("GetReporteMantenimientoTags")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
//.AllowAnonymous();

app.MapGet("/Reportes/ActividadUsuario", async (string? dia, string? semana, string? mes, string? nombre, string? rol, string? accion, IReportesService _reportesService, ILogger<Program> _logger) =>
{
    try
    {
        dia = GetNullableString(dia);
        mes = GetNullableString(mes);
        semana = GetNullableString(semana);

        string patternDia = @"(19|20)\d\d[-/.](0[1-9]|1[012])[-/.](0[1-9]|[1][0-9]|[2][0-9]|3[01])";

        if (dia != null)
        {
            if (Regex.IsMatch(dia, patternDia) == false)
            {
                return Results.Problem("El dia se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        string patternMes = @"(19|20)\d\d[-/.](0[1-9]|1[012])";

        if (mes != null)
        {
            if (Regex.IsMatch(mes, patternMes) == false)
            {
                return Results.Problem("El mes se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        string patternSemana = @"(19|20)\d\d[-/.](W0[1-9]|W1[0-9]|W2[0-9]|W3[0-9]|W4[0-9]|W5[0-3])";

        if (semana != null)
        {
            if (Regex.IsMatch(semana, patternSemana) == false)
            {
                return Results.Problem("La semana se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        if ((dia != null && semana != null) || (dia != null && mes != null) || (mes != null && semana != null))
            return Results.Problem("Mas de una fecha ingresada", statusCode: 400);

        var newModule = await _reportesService.GetReportesActividadUsuarios(dia, semana, mes, nombre, rol, accion);
        if (newModule == null) return Results.NoContent();
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
.WithName("GetReportesActividadUsuarios")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

#endregion Reportes

#region Modules

app.MapGet("/module/{id}", async (int id, IModulesService _modulesService, ILogger<Program> _logger) =>
{
    try
    {
        var module = await _modulesService.GetModuleAsync(id);
        if (module == null) return Results.NotFound();
        return Results.Ok(module);
    }
    catch (Exception e)
    {
        _logger.LogError(e, e.Message);
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("GetModule")
.Produces<List<Module>>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapGet("/modules", async (string? role, IModulesService _modulesService, ILogger<Program> _logger) =>
{
    try
    {
        var modules = await _modulesService.GetModulesAsync(role);
        return Results.Ok(modules);
    }
    catch (Exception e)
    {
        _logger.LogError(e, e.Message);
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("GetModules")
.Produces<List<Module>>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapPost("/modulesrole", async (RoleModules roleModules, IModulesService _modulesService, ILogger<Program> _logger) =>
{
    try
    {
        var res = await _modulesService.PostRoleModulesAsync(roleModules);
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
.WithName("PostRoleModules")
.Produces<RoleModules>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

#endregion

#region Tags

app.MapGet("/tags",
    async (int? paginaActual, int? numeroDeFilas, string? tag, bool? estatus, DateTime? fecha, string? noDePlaca, string? noEconomico, ITagsService _tagsService, ILogger<Program> _logger) =>
{
    try
    {
        noDePlaca = GetNullableString(noDePlaca);
        noEconomico = GetNullableString(noEconomico);

        var tags = await _tagsService.GetTagsAsync(paginaActual, numeroDeFilas, tag, estatus, fecha, noDePlaca, noEconomico);
        return Results.Ok(tags);
    }
    catch (Exception e)
    {
        _logger.LogError(e, e.Message);
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);

    }
})
.WithName("GetTags")
.Produces<List<TagList>>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapPut("/tag/{id}",
    async (string id, TagList tag, ITagsService _tagsService, ILogger<Program> _logger) =>
    {
        try
        {
            if (id != tag.Tag) return Results.BadRequest();
            if (await _tagsService.GetTagsCountAsync(tag.Tag, null, null, null, null) <= 0) return Results.NotFound();
            var res = await _tagsService.UpdateTagAsync(tag);
            if (res) return Results.NoContent();
            return Results.BadRequest();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            if (e.GetType() == typeof(ValidationException))
                return Results.Problem(e.Message, statusCode: 400);
            return Results.Problem(e.Message);

        }
    })
.WithName("UpdateTag")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status400BadRequest)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapPost("/tag",
    async (TagList tag, ITagsService _tagsService, ILogger<Program> _logger) =>
    {
        try
        {
            var res = await _tagsService.CreateTagAsync(tag);
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
.WithName("CreateTag")
.Produces<TagList>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapDelete("/tag/{id}",
    async (string id, ITagsService _tagsService, ILogger<Program> _logger) =>
    {
        try
        {
            if (await _tagsService.GetTagsCountAsync(id, null, null, null, null) <= 0) return Results.NotFound();
            var res = await _tagsService.DeleteTagAsync(id);
            if (res) return Results.NoContent();
            return Results.BadRequest();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            if (e.GetType() == typeof(ValidationException))
                return Results.Problem(e.Message, statusCode: 400);
            return Results.Problem(e.Message);

        }
    })
.WithName("DeleteTag")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status400BadRequest)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapGet("/tagsCount",
    async (string? tag, bool? estatus, DateTime? fecha, ITagsService _tagsService, string? noDePlaca, string? noEconomico, ILogger<Program> _logger) =>
    {
        try
        {
            var tags = await _tagsService.GetTagsCountAsync(tag, estatus, fecha, noDePlaca, noEconomico);
            return Results.Ok(tags);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            if (e.GetType() == typeof(ValidationException))
                return Results.Problem(e.Message, statusCode: 400);
            return Results.Problem(e.Message);

        }
    })
.WithName("GetTagsCount")
.Produces<int>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

#endregion

app.MapGet("/lanes",
    async (ITelepeajeService _telepeajeService, ILogger<Program> _logger) =>
    {
        try
        {
            var lanes = await _telepeajeService.GetLanes();
            return Results.Ok(lanes);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            if (e.GetType() == typeof(ValidationException))
                return Results.Problem(e.Message, statusCode: 400);
            return Results.Problem(e.Message);

        }
    })
.WithName("GetLanes")
.Produces<List<LaneCatalog>>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");


app.MapGet("/class",
    async (ITelepeajeService _telepeajeService, ILogger<Program> _logger) =>
    {
        try
        {
            var clase = await _telepeajeService.GetClass();
            return Results.Ok(clase);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            if (e.GetType() == typeof(ValidationException))
                return Results.Problem(e.Message, statusCode: 400);
            return Results.Problem(e.Message);

        }
    })
.WithName("GetClass")
.Produces<List<TypeClass>>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapGet("/transactions",
    async (int? paginaActual, int? numeroDeFilas, string? tag, string? carril, string? cuerpo, DateTime? fecha, string? noDePlaca, string? noEconomico, string? clase, ITelepeajeService _telepeajeService, ILogger<Program> _logger) =>
    {
        try
        {
            var transactions = await _telepeajeService.GetTransactions(paginaActual, numeroDeFilas, tag, carril, cuerpo, fecha, noDePlaca, noEconomico, clase);
            return Results.Ok(transactions);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            if (e.GetType() == typeof(ValidationException))
                return Results.Problem(e.Message, statusCode: 400);
            return Results.Problem(e.Message);

        }
    })
.WithName("GetTransactions")
.Produces<List<Transaction>>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapGet("/cruces",
    async (int? paginaActual, int? numeroDeFilas, string? tag, string? carril, string? cuerpo, DateTime? fecha, string? noDePlaca, string? noEconomico, string? clase, ITelepeajeService _telepeajeService, ILogger<Program> _logger) =>
    {
        try
        {
            var cruces = await _telepeajeService.GetCruces(paginaActual, numeroDeFilas, tag, carril, cuerpo, fecha, noDePlaca, noEconomico, clase);
            return Results.Ok(cruces);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            if (e.GetType() == typeof(ValidationException))
                return Results.Problem(e.Message, statusCode: 400);
            return Results.Problem(e.Message);

        }
    })
.WithName("GetCruces")
.Produces<List<Cruce>>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
//.AllowAnonymous(); 

app.MapGet("/transactionsCount",
    async (string? tag, string? carril, string? cuerpo, DateTime? fecha, string? noDePlaca, string? noEconomico, string? clase, ITelepeajeService _telepeajeService, ILogger<Program> _logger) =>
    {
        try
        {
            var tags = await _telepeajeService.GetTransactionsCount(tag, carril, cuerpo, fecha, noDePlaca, noEconomico, clase);
            return Results.Ok(tags);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            if (e.GetType() == typeof(ValidationException))
                return Results.Problem(e.Message, statusCode: 400);
            return Results.Problem(e.Message);

        }
    })
.WithName("GetTransactionsCount")
.Produces<int>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapGet("/turnos",
    async (DateTime fecha, ITelepeajeService _telepeajeService, ILogger<Program> _logger) =>
    {
        try
        {
            var lanes = await _telepeajeService.GetTurnos(fecha);
            return Results.Ok(lanes);
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
.Produces<int[]>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

//new endpoint
app.MapGet("/ViaPassTags",    
    async (string? tag, ITagsService _tagsService, ILogger<Program> _logger) =>
    {
        try
        {
            var tags = await _tagsService.GetViaPassTagsAsync(tag);

            if(!string.IsNullOrWhiteSpace(tag) && tags.Count() == 0)
            {
                return Results.Problem(statusCode: 404);
            }

            return Results.Ok(tags);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Results.Problem(e.Message);
        }
    })
.WithName("GetViaPassTags")
.Produces<IEnumerable<Viapasstags>>(StatusCodes.Status200OK)
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapGet("/ActividadUsuario", async (int? paginaActual, int? numeroDeFilas, string? dia, string? semana, string? mes, string? nombre, string? rol, string? accion, IReportesService _reportesService, ILogger<Program> _logger, AuxiliaryMethods _auxiliaryMethods) =>
{
    try
    {
        dia = GetNullableString(dia);
        mes = GetNullableString(mes);
        semana = GetNullableString(semana);

        string patternDia = @"(19|20)\d\d[-/.](0[1-9]|1[012])[-/.](0[1-9]|[1][0-9]|[2][0-9]|3[01])";

        if (dia != null)
        {
            if (Regex.IsMatch(dia, patternDia) == false)
            {
                return Results.Problem("El dia se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        string patternMes = @"(19|20)\d\d[-/.](0[1-9]|1[012])";

        if (mes != null)
        {
            if (Regex.IsMatch(mes, patternMes) == false)
            {
                return Results.Problem("El mes se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        string patternSemana = @"(19|20)\d\d[-/.](W0[1-9]|W1[0-9]|W2[0-9]|W3[0-9]|W4[0-9]|W5[0-3])";

        if (semana != null)
        {
            if (Regex.IsMatch(semana, patternSemana) == false)
            {
                return Results.Problem("La semana se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        if ((dia != null && semana != null) || (dia != null && mes != null) || (mes != null && semana != null))
            return Results.Problem("Mas de una fecha ingresada", statusCode: 400);

        var fecha = _auxiliaryMethods.ObtenerFechas(dia, mes, semana);
        var newModule = await _reportesService.GetActividadUsuarios(fecha, nombre, rol, accion); 

        if (paginaActual != null && numeroDeFilas != null)
        {
            newModule = newModule.Skip((int)((paginaActual - 1) * numeroDeFilas)).Take((int)numeroDeFilas).ToList();
        }

        if (newModule == null) return Results.NoContent();
        return Results.Ok(newModule);
    }
    catch (Exception e)
    {
        _logger.LogError(e, e.Message);
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("GetActividadUsuarios")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

app.MapGet("/ActividadUsuarioCount", async (string? dia, string? semana, string? mes, string? nombre, string? rol, string? accion, IReportesService _reportesService, ILogger<Program> _logger, AuxiliaryMethods _auxiliaryMethods) =>
{
    try
    {
        dia = GetNullableString(dia);
        mes = GetNullableString(mes);
        semana = GetNullableString(semana);

        string patternDia = @"(19|20)\d\d[-/.](0[1-9]|1[012])[-/.](0[1-9]|[1][0-9]|[2][0-9]|3[01])";

        if (dia != null)
        {
            if (Regex.IsMatch(dia, patternDia) == false)
            {
                return Results.Problem("El dia se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        string patternMes = @"(19|20)\d\d[-/.](0[1-9]|1[012])";

        if (mes != null)
        {
            if (Regex.IsMatch(mes, patternMes) == false)
            {
                return Results.Problem("El mes se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        string patternSemana = @"(19|20)\d\d[-/.](W0[1-9]|W1[0-9]|W2[0-9]|W3[0-9]|W4[0-9]|W5[0-3])";

        if (semana != null)
        {
            if (Regex.IsMatch(semana, patternSemana) == false)
            {
                return Results.Problem("La semana se encuentra en un formato incorrecto", statusCode: 400);
            }
        }

        if ((dia != null && semana != null) || (dia != null && mes != null) || (mes != null && semana != null))
            return Results.Problem("Mas de una fecha ingresada", statusCode: 400);

        var fecha = _auxiliaryMethods.ObtenerFechas(dia, mes, semana);
        var newModule = await _reportesService.GetActividadUsuarios(fecha, nombre, rol, accion);

        //if (paginaActual != null && numeroDeFilas != null)
        //{
        //    newModule = newModule.Skip((int)((paginaActual - 1) * numeroDeFilas)).Take((int)numeroDeFilas).ToList();
        //}

        if (newModule == null) return Results.NoContent();
        return Results.Ok(newModule.Count);
    }
    catch (Exception e)
    {
        _logger.LogError(e, e.Message);
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
.WithName("GetActividadUsuariosCount")
.Produces<IResult>(StatusCodes.Status200OK, "application/pdf")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
.Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");


static string? GetNullableString(string? value) => !string.IsNullOrWhiteSpace(value) && value.ToUpper().Contains("NULL") ? null : value;

app.Run();
