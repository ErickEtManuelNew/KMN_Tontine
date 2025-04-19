using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using KMN_Tontine.Application.Interfaces;

namespace KMN_Tontine.Application.Services.Scheduled
{
    public class EmailCheckerBackgroundService : BackgroundService
    {
        private readonly ILogger<EmailCheckerBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5);

        public EmailCheckerBackgroundService(
            ILogger<EmailCheckerBackgroundService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var emailChecker = scope.ServiceProvider.GetRequiredService<IEmailCheckerService>();
                        await emailChecker.CheckEmailsAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while checking emails");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
} 