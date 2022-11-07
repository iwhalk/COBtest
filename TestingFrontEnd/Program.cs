using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TestingFrontEnd;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using TanvirArjel.Blazor.DependencyInjection;
using Microsoft.Fast.Components.FluentUI;
using TestingFrontEnd.Stores;
//using TestingFrontEnd.Services;
using TestingFrontEnd.Interfaces;
using Microsoft.JSInterop;
using TestingFrontEnd.Repositories;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Components;
using TestingFrontEnd.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var client = "blazor-client";

if (!builder.HostEnvironment.IsDevelopment())
{
    client = "blazor-azure";
}

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddHttpClient("ApiGateway")
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
builder.Services.AddSingleton(serviceProvider => (IJSInProcessRuntime)serviceProvider.GetRequiredService<IJSRuntime>());

builder.Services.AddComponents();
builder.Services.AddFluentUIComponents();

builder.Services.AddScoped<ApplicationContext>();
builder.Services.AddScoped<IGenericRepository, GenericRepository>();

builder.Services.AddScoped<ILessorService, LessorService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IPropertyTypeService, PropertyTypeService>();
builder.Services.AddScoped<IServicesService, ServicesService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IFeaturesService, FeaturesService>();
builder.Services.AddScoped<IDescriptionService, DescriptionService>();


// Supply HttpClient instances that include access tokens when making requests to the server project.
builder.Services.AddScoped(provider =>
{
    var factory = provider.GetRequiredService<IHttpClientFactory>();
    return factory.CreateClient("ApiGateway");
});
builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.ClientId = client;
    options.ProviderOptions.Authority = builder.HostEnvironment.BaseAddress;
    options.ProviderOptions.ResponseType = "code";

    // Note: response_mode=fragment is the best option for a SPA. Unfortunately, the Blazor WASM
    // authentication stack is impacted by a bug that prevents it from correctly extracting
    // authorization error responses (e.g error=access_denied responses) from the URL fragment.
    // For more information about this bug, visit https://github.com/dotnet/aspnetcore/issues/28344.
    //
    options.ProviderOptions.ResponseMode = "query";
    options.AuthenticationPaths.RemoteRegisterPath = $"https://localhost:7140/Identity/Account/Register";

    // Add the "roles" (OpenIddictConstants.Scopes.Roles) scope and the "role" (OpenIddictConstants.Claims.Role) claim
    // (the same ones used in the Startup class of the Server) in order for the roles to be validated.
    // See the Counter component for an example of how to use the Authorize attribute with roles
    options.ProviderOptions.DefaultScopes.Add("roles");
    options.UserOptions.RoleClaim = "role";
});

await builder.Build().RunAsync();
