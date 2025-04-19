using System.Text;

using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Application.Mappings;
using KMN_Tontine.Application.Seed;
using KMN_Tontine.Application.Services;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Shared.Enums;
using KMN_Tontine.Domain.Interfaces;
using KMN_Tontine.Infrastructure.Data;
using KMN_Tontine.Infrastructure.Interface;
using KMN_Tontine.Infrastructure.Repositories.Implementations;
using KMN_Tontine.Infrastructure.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Serilog;
using KMN_Tontine.Shared.Options;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName;
Console.WriteLine($"🚀 Démarrage en mode : {environment}");

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? Environment.GetEnvironmentVariable("ConnectionStrings_DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("❌ ERREUR : La connexion SQL Server est introuvable !");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<ITontineRepository, TontineRepository>();
builder.Services.AddScoped<IPaymentPromiseRepository, PaymentPromiseRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Enregistrement des services métiers
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPaymentPromiseService, PaymentPromiseService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.Configure<EmailCheckerOptions>(
    builder.Configuration.GetSection("EmailChecker"));

builder.Services.AddSingleton<IEmailCheckerService, EmailCheckerService>();
//builder.Services.AddHostedService<EmailCheckerBackgroundService>();



builder.Services.AddAutoMapper(typeof(MappingProfile));
//builder.Services.AddAutoMapper(typeof(TransactionProfile));

builder.Services.AddIdentity<Member, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 🔥 Charger les User Secrets en mode développement
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"]
             ?? Environment.GetEnvironmentVariable("Jwt_Key");

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
    options.RequireHttpsMetadata = environment == "Production";
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true
    };
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Host.UseSerilog((ctx, lc) =>
    lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

// Enregistrement des seeders
builder.Services.AddScoped<RoleSeeder>();
builder.Services.AddScoped<SuperAdminSeeder>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddHttpClient();

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

var baseUrl = builder.Configuration["ApiSettings:BaseUrl"]
              ?? Environment.GetEnvironmentVariable("BaseUrl");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "KMN-Tontine API v1");
    c.RoutePrefix = string.Empty;
});

// 🔥 Exécuter les migrations DB automatiquement
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    if (!dbContext.Tontines.Any())
    {
        var tontine = new Tontine
        {
            Name = "KMN Ndjangui",
            Address = "1, rue de Ngualan",
            Email = "kmn_ndjangui@ndjangui.com",
            CreationDate = DateTime.UtcNow,
            IsActive = true
        };
        dbContext.Tontines.Add(tontine);

        await dbContext.SaveChangesAsync();

        // 🔁 Créer les types de comptes pour la tontine
        var accountTypes = Enum.GetValues(typeof(AccountType)).Cast<AccountType>();

        foreach (var type in accountTypes)
        {
            dbContext.Accounts.Add(new Account
            {
                Type = type,
                Balance = 0,
                Comment = $"Compte initial {type}",
                MemberId = null, // compte association
                TontineId = tontine.Id
            });
        }

        await dbContext.SaveChangesAsync();
    }
}

// Appeler les seeders
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        // Résolution des seeders via DI
        var roleSeeder = services.GetRequiredService<RoleSeeder>();
        await roleSeeder.SeedRolesAsync();

        var superAdminSeeder = services.GetRequiredService<SuperAdminSeeder>();
        await superAdminSeeder.SeedSuperAdminAsync();
    }
    catch (Exception ex)
    {
        // Gérer les erreurs lors du seeding
        Console.WriteLine($"An error occurred while seeding: {ex.Message}");
    }
}

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.Urls.Add($"{baseUrl}{port}");
}
else
{
    // En prod, Docker ou Railway : écouter sur HTTP
    app.Urls.Clear();
    app.Urls.Add($"{baseUrl}{port}");
}

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine($"🚀 Application démarrée sur : {string.Join(", ", app.Urls)}");

app.Run();

