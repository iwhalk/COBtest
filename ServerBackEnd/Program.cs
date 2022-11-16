using ApiGateway;
using ApiGateway.Data;
using ApiGateway.Interfaces;
using ApiGateway.Models;
using ApiGateway.Services;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
//using static ApiGateway.Services.UsuariosService;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var secretKey = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("SecretKey"));
var key = new SymmetricSecurityKey(secretKey);
var certificateThumbprint = "A2956D21BD9AD1B06AD9DBE8949782B0D8210948";
X509Certificate2? certificate = null;
    
var securityScheme = new OpenApiSecurityScheme()
{
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "JWT Authentication"
};

var securityRequirements = new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference()
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
    }
};
var openApiInfo = new OpenApiInfo()
{
    Version = "1.0",
    Title = "API Gateway",
    Description = ""
};
if (builder.Environment.IsDevelopment())
{
    certificate = new X509Certificate2(
    "cert.pfx",
    "12345"
);
}
else
{
    byte[]? bytes = null;
    try
    {
        bytes = File.ReadAllBytes($"/var/ssl/private/{certificateThumbprint}.p12");
        Console.WriteLine(bytes);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    if(bytes != null)
    certificate = new X509Certificate2(bytes);
}

builder.Services.AddProblemDetails(setup =>
{
    setup.IncludeExceptionDetails = (ctx, env) => builder.Environment.IsDevelopment() || builder.Environment.IsStaging();
    setup.Rethrow<NotSupportedException>();
    setup.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
    setup.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
    setup.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
})
.AddControllers()
    // Adds MVC conventions to work better with the ProblemDetails middleware.
    .AddProblemDetailsConventions()
.AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpClient("Reportes", client => client.BaseAddress = new Uri("http://localhost:7293/"));
}
else
{
    builder.Services.AddHttpClient("Reportes", client => client.BaseAddress = new Uri("https://reportesarisoft2245.azurewebsites.net"));
}

builder.Services.AddCors();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.UseOpenIddict();
});

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

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
            IssuerSigningKey = key,
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(50);
        options.SlidingExpiration = false;
    });

builder.Services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.UserNameClaimType = Claims.Name;
    options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
    options.ClaimsIdentity.RoleClaimType = Claims.Role;
    options.ClaimsIdentity.EmailClaimType = Claims.Email;
});

builder.Services.AddQuartz(options =>
{
    options.UseMicrosoftDependencyInjectionJobFactory();
    options.UseSimpleTypeLoader();
    options.UseInMemoryStore();
});
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

builder.Services.AddOpenIddict()

        // Register the OpenIddict core components.
        .AddCore(options =>
        {
            options.UseEntityFrameworkCore()
                .UseDbContext<ApplicationDbContext>();

            options.UseQuartz();
        })

        // Register the OpenIddict server components.
        .AddServer(options =>
        {
            options
                .AllowAuthorizationCodeFlow()
                .AllowPasswordFlow()
                .AllowRefreshTokenFlow()
                .AcceptAnonymousClients();

            options
                .SetAuthorizationEndpointUris("/api/identity/authorize")
                .SetLogoutEndpointUris("/api/identity/logout")
                .SetTokenEndpointUris("/api/identity/token")
                .SetUserinfoEndpointUris("/api/identity/userinfo");

            options
                .RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

            // Encryption and signing of tokens
            if (certificate == null)
            {
                options
                    .AddEphemeralEncryptionKey()
                    .AddDevelopmentSigningCertificate();
            }
            else
            {
                options
                    .AddEncryptionCertificate(certificate)
                    .AddSigningCertificate(certificate)
                    .DisableAccessTokenEncryption()
                    .AddSigningKey(key);
            }

            // Register scopes (permissions)
            options
                .RegisterScopes("api");

            // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
            options
                .UseAspNetCore()
                .EnableAuthorizationEndpointPassthrough()
                .EnableLogoutEndpointPassthrough()
                .EnableStatusCodePagesIntegration()
                .EnableTokenEndpointPassthrough();
        })
        .AddValidation(options =>
        {
            options.UseLocalServer();
            options.Configure(o => o.TokenValidationParameters.IssuerSigningKey = key);
            options.UseAspNetCore();
        });


builder.Services.AddLogging(loggingBuilder =>
{
    var loggingSection = builder.Configuration.GetSection("Logging");
    loggingBuilder.AddFile(loggingSection, fileLoggerOpts => {
        fileLoggerOpts.FormatLogEntry = (msg) =>
        {
            var sb = new System.Text.StringBuilder();
            StringWriter sw = new StringWriter(sb);
            var jsonWriter = new Newtonsoft.Json.JsonTextWriter(sw);
            jsonWriter.WriteStartArray();
            jsonWriter.WriteValue(DateTime.Now.ToString("o"));
            jsonWriter.WriteValue(msg.LogLevel.ToString());
            jsonWriter.WriteValue(msg.LogName);
            jsonWriter.WriteValue(msg.EventId.Id);
            jsonWriter.WriteValue(msg.Message);
            jsonWriter.WriteValue(msg.Exception?.ToString());
            jsonWriter.WriteEndArray();
            return sb.ToString();
        };
});
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", openApiInfo);
    options.AddSecurityDefinition("Bearer", securityScheme);
    options.AddSecurityRequirement(securityRequirements);
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddMediatR(Assembly.Load("ApiGateway"));

//builder.Services.AddHostedService<Worker>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<ILessorService, LessorService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IPropertyTypeService, PropertyTypeService>();
builder.Services.AddScoped<IServicesService, ServicesService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IFeaturesService, FeaturesService>();
builder.Services.AddScoped<IDescriptionService, DescriptionService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IBlobsService, BlobsService>();
builder.Services.AddScoped<IReportesService, ReportesService>();
builder.Services.AddScoped<IBlobInventoryService, BlobInventoryService>();
builder.Services.AddScoped<IReceptionCertificateService, ReceptionCertificateService>();
builder.Services.AddScoped<IAspNetUserService, AspNetUserService>();
builder.Services.AddScoped<IMailAriService, MailAriService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseProblemDetails();
app.UseBlazorFrameworkFiles();

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.UseEndpoints(options =>
{
    options.MapRazorPages();
    options.MapControllers();
    options.MapFallbackToFile("index.html");
});

app.MapControllers();

app.Run();
