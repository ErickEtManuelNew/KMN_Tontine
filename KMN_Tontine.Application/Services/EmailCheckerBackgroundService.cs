using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using KMN_Tontine.Application.Interfaces;

namespace KMN_Tontine.Application.Services
{
    public class EmailCheckerBackgroundService : BackgroundService
    {
        private readonly IEmailCheckerService _emailCheckerService;
        private readonly int _checkIntervalMinutes;

        public EmailCheckerBackgroundService(
            IEmailCheckerService emailCheckerService,
            IConfiguration configuration)
        {
            _emailCheckerService = emailCheckerService;
            _checkIntervalMinutes = configuration.GetValue<int>("EmailChecker:CheckIntervalMinutes", 5);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _emailCheckerService.CheckEmailsAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur dans le service de vérification d'emails : {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(_checkIntervalMinutes), stoppingToken);
            }
        }
    }
} 