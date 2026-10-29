using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GestionOrdinateurs.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredSessionStorage();

// client http pour l'api d'auth (direct, pas via la gateway)
builder.Services.AddHttpClient("AuthentificationApi", c =>
{
    c.BaseAddress = new Uri("http://localhost:5100");
});

// client http pour les calls qui passent par la gateway
builder.Services.AddHttpClient("GatewayClient", c =>
{
    c.BaseAddress = new Uri("http://localhost:5010");
});

// client direct vers l'api pour vérifier le login
builder.Services.AddHttpClient("GestionOrdinateursApi", c =>
{
    c.BaseAddress = new Uri("http://localhost:5038");
});

await builder.Build().RunAsync();
