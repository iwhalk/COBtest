using Obra.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using TanvirArjel.Blazor.DependencyInjection;
using Microsoft.Fast.Components.FluentUI;
using Obra.Client.Interfaces;
using Obra.Client.Repositories;
using Obra.Client.Services;
using Obra.Client.Stores;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("ApiGateway", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("ApiGateway"));

builder.Services.AddSingleton(serviceProvider => (IJSInProcessRuntime)serviceProvider.GetRequiredService<IJSRuntime>());

builder.Services.AddComponents();
builder.Services.AddFluentUIComponents();

// Supply HttpClient instances that include access tokens when making requests to the server project

builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.ClientId = "blazor-soft2245pruebas";
    //options.ProviderOptions.ClientId = "blazor-client";
    options.ProviderOptions.Authority = builder.HostEnvironment.BaseAddress;
    options.ProviderOptions.ResponseType = "code";

    // Note: response_mode=fragment is the best option for a SPA. Unfortunately, the Blazor WASM
    // authentication stack is impacted by a bug that prevents it from correctly extracting
    // authorization error responses (e.g error=access_denied responses) from the URL fragment.
    // For more information about this bug, visit https://github.com/dotnet/aspnetcore/issues/28344.
    //
    options.ProviderOptions.ResponseMode = "query";
    options.AuthenticationPaths.RemoteRegisterPath = $"{builder.HostEnvironment.BaseAddress}/Identity/Account/Register";

    // Add the "roles" (OpenIddictConstants.Scopes.Roles) scope and the "role" (OpenIddictConstants.Claims.Role) claim
    // (the same ones used in the Startup class of the Server) in order for the roles to be validated.
    // See the Counter component for an example of how to use the Authorize attribute with roles
    options.ProviderOptions.DefaultScopes.Add("roles");
    options.UserOptions.RoleClaim = "role";
});

builder.Services.AddApiAuthorization();

builder.Services.AddScoped<ApplicationContext>();
builder.Services.AddScoped<IGenericRepository, GenericRepository>();

builder.Services.AddScoped<IBuildingsService, BuildingsService>();
builder.Services.AddScoped<IAreasService, AreasService>();
builder.Services.AddScoped<IActivitiesService, ActivitiesService>();
builder.Services.AddScoped<IApartmentsService, ApartmentsService>();
builder.Services.AddScoped<IElementsService, ElementsService>();
builder.Services.AddScoped<ISubElementsService, SubElementsService>();
builder.Services.AddScoped<IProgressLogsService, ProgressLogsService>();
builder.Services.AddScoped<IBlobsService, BlobsService>();
builder.Services.AddScoped<IProgressReportService, ProgressReportService>();

await builder.Build().RunAsync();
