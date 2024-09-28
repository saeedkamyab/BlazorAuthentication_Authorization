using Blazored.LocalStorage;
using JwtBlazorWithGpt.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Headers;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddTransient<JwtAuthorizationMessageHandler>();

builder.Services.AddHttpClient("YourApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7186/");
})
.AddHttpMessageHandler<JwtAuthorizationMessageHandler>();


//builder.Services.AddScoped(sp => new HttpClient
//{
//    BaseAddress = new Uri("https://localhost:7186/"),
//}).AddHttpMessageHandler<JwtAuthorizationMessageHandler>();


await builder.Build().RunAsync();
