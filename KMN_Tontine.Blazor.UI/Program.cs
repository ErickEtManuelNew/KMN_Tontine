using Blazored.LocalStorage;

using KMN_Tontine.Blazor.UI.Services;
using KMN_Tontine.Blazor.UI.Services.Authentication;
using KMN_Tontine.Blazor.UI.Services.Base;

using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// 🌍 Détection de l'environnement
var environment = builder.Environment.EnvironmentName;
Console.WriteLine($"🚀 Blazor Server démarre en mode : {environment}");

// 🔥 Charger les User Secrets en développement
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// 🔗 Définir l'URL de l'API (Railway en prod, localhost en dev)
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]
                 ?? Environment.GetEnvironmentVariable("ApiSettings_BaseUrl")
                 ?? "https://localhost:5000";

Console.WriteLine($"🔗 API utilisée : {apiBaseUrl}");

builder.Services.AddHttpClient<IClient, Client>(cl => cl.BaseAddress = new Uri(apiBaseUrl));

// 🔑 Gestion des tokens et de l'authentification
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p =>
                p.GetRequiredService<ApiAuthenticationStateProvider>());
builder.Services.AddScoped<ICompteService, CompteService>();

builder.Services.AddScoped<CurrentUserService>();

// 🗄️ Stockage local pour conserver le token JWT
builder.Services.AddBlazoredLocalStorage();

// Add services to the container.
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();

var app = builder.Build();

// 🌍 Configuration du pipeline
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

Console.WriteLine($"🚀 Blazor Server est prêt sur {app.Environment.EnvironmentName}");

app.Run();
