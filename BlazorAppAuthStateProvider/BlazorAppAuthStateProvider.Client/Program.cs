global using Microsoft.AspNetCore.Components.Authorization;
using BlazorAppAuthStateProvider.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();



await builder.Build().RunAsync();
