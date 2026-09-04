using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using YaVoy;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<YaVoy.Services.ZonasService>();
builder.Services.AddScoped<YaVoy.Services.RutaService>();
builder.Services.AddScoped<YaVoy.Services.TarifaService>();
builder.Services.AddScoped<YaVoy.Services.TasaEuroService>();

await builder.Build().RunAsync();
