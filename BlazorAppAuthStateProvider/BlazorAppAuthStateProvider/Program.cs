using BlazorAppAuthStateProvider.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BlazorAppAuthStateProvider.Components;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlazorAppAuthStateProvider.Client.AuthProvider;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddBlazoredLocalStorage();



builder.Services.AddAuthentication();//Mr.Sk Added
builder.Services.AddAuthorization();//Mr.Sk Added
builder.Services.AddAuthorizationCore();//Mr.Sk Added

builder.Services.AddTransient<AuthenticationStateProvider, CustomAuthStateProvider>();


builder.Services.AddCascadingAuthenticationState();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();



app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorAppAuthStateProvider.Client._Imports).Assembly);


app.Run();
