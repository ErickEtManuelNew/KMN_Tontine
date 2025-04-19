using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using KMN_Tontine.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KMN_Tontine.Application.Services.Scheduled
{
    public class EmailPaymentValidatorBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EmailCheckerBackgroundService> _logger;
        private readonly IEmailCheckerService _emailCheckerService;
        private readonly int _checkIntervalMinutes;

        public EmailPaymentValidatorBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<EmailCheckerBackgroundService> logger,
            IEmailCheckerService emailCheckerService,
            IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _emailCheckerService = emailCheckerService;
            _checkIntervalMinutes = configuration.GetValue<int>("EmailChecker:CheckIntervalMinutes", 5);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var checker = scope.ServiceProvider.GetRequiredService<IEmailCheckerService>();
                    await checker.CheckEmailsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur dans le service de vérification d'emails");
                }

                await Task.Delay(TimeSpan.FromMinutes(_checkIntervalMinutes), stoppingToken);
            }
        }
    }
} 