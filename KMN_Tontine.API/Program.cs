using System.Text;

using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Application.Mappings;
using KMN_Tontine.Application.Services;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Infrastructure.Data;
using KMN_Tontine.Infrastructure.Repositories.Implementations;
using KMN_Tontine.Infrastructure.Repositories.Interfaces;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog; // Ajouté pour résoudre l'erreur

var builder = WebApplication.CreateBuilder(args);

// Connexion DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Enregistrement des services
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
//TODO: Ajouter les services manquants
//builder.Services.AddScoped<ICompteRepository, CompteRepository>();
//builder.Services.AddScoped<IMembreRepository, MembreRepository>();

// Enregistrement de AutoMapper
builder.Services.AddAutoMapper(typeof(TransactionProfile));

// Configuration Identity
builder.Services.AddIdentity<Membre, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IAssociationRepository, AssociationRepository>();
builder.Services.AddScoped<IMembreRepository, MembreRepository>();
builder.Services.AddScoped<MembreService>();

// Configuration JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"];

if (string.IsNullOrEmpty(jwtKey))
{
    jwtKey = builder.Configuration["Jwt:Key"] ?? Environment.GetEnvironmentVariable("Jwt__Key");
}

if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("❌ ERREUR : La clé JWT est introuvable !");
}

var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"], // 🔥 Vérifie l'émetteur
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"], // 🔥 Vérifie l'audience
            ValidateLifetime = true
        };
    });
builder.Services.AddAuthorization();

// Enregistrement des services métiers
builder.Services.AddScoped<IMembreService, MembreService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => // Ajouté pour configurer Swagger
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "KMN_Tontine.API", Version = "v1" });
    // Ajouter l'option d'authentification avec un bouton "Authorize"
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Entrez votre token JWT ici : Bearer {votre_token}"
    });

    // Exiger le token pour les endpoints sécurisés
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Host.UseSerilog((ctx, lc) =>
    lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "KMN-Tontine API v1");
        c.RoutePrefix = string.Empty; // Ouvre Swagger directement sur la racine (`http://localhost:5000/`)
    });
}

//app.Use(async (context, next) =>
//{
//    Console.WriteLine($"🔍 Requête reçue : {context.Request.Method} {context.Request.Path}");
//    if (context.Request.Headers.ContainsKey("Authorization"))
//    {
//        Console.WriteLine($"✅ Token détecté : {context.Request.Headers["Authorization"]}");
//    }
//    else
//    {
//        Console.WriteLine("❌ Aucun token JWT envoyé !");
//    }
//    await next();
//});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
