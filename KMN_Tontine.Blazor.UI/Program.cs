using Blazored.LocalStorage;
using KMN_Tontine.Blazor.UI;
using KMN_Tontine.Blazor.UI.Services;
using KMN_Tontine.Blazor.UI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// 🌍 Détection de l’environnement
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

// 📡 Configuration du service HTTP avec injection automatique du token JWT
builder.Services.AddHttpClient("KMNTontineAPI", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<AuthenticationHeaderHandler>();

builder.Services.AddScoped<IClient>(sp =>
{
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("KMNTontineAPI");
    return new Client(apiBaseUrl, httpClient);
});

// 🔑 Gestion des tokens et de l'authentification
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<ICompteService, CompteService>();

// 🔥 Gestion automatique de l'ajout du token dans les requêtes HTTP
builder.Services.AddScoped<AuthenticationHeaderHandler>();

// 🗄️ Stockage local pour conserver le token JWT
builder.Services.AddBlazoredLocalStorage();

// Add services to the container.
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();

var app = builder.Build();

// 🌍 Configuration du pipeline
if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

Console.WriteLine($"🚀 Blazor Server est prêt sur {app.Environment.EnvironmentName}");

app.Run();
