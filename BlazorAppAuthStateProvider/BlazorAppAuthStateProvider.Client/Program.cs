global using Microsoft.AspNetCore.Components.Authorization;
using BlazorAppAuthStateProvider.Client;
using BlazorAppAuthStateProvider.Client.AuthProvider;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

//builder.Services.AddScoped<AuthenticationStateProvider, MyStateProvider>();


builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

builder.Services.AddBlazoredLocalStorage();



await builder.Build().RunAsync();
