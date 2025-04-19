using System;
using System.Threading.Tasks;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Shared.DTOs.Requests;
using KMN_Tontine.Shared.DTOs.Responses;
using Microsoft.Extensions.Logging;

namespace KMN_Tontine.Application.Services.Scheduled
{
    public class EmailPaymentValidatorService : IEmailPaymentValidatorService
    {
        private readonly IEmailCheckerService _emailCheckerService;
        private readonly IPaymentPromiseService _paymentPromiseService;
        private readonly ILogger<EmailPaymentValidatorService> _logger;

        public EmailPaymentValidatorService(
            IEmailCheckerService emailCheckerService,
            IPaymentPromiseService paymentPromiseService,
            ILogger<EmailPaymentValidatorService> logger)
        {
            _emailCheckerService = emailCheckerService;
            _paymentPromiseService = paymentPromiseService;
            _logger = logger;
        }

        public async Task CheckEmailsAsync()
        {
            try
            {
                var transfers = await _emailCheckerService.GetTransfersAsync();
                foreach (var transfer in transfers)
                {
                    try
                    {
                        var transferInfo = await _emailCheckerService.GetTransferDetailsAsync(transfer);
                        if (transferInfo == null || !transferInfo.PromiseId.HasValue)
                            continue;

                        var promise = await _paymentPromiseService.GetPaymentPromiseByIdAsync(transferInfo.PromiseId.Value);
                        if (promise == null)
                        {
                            _logger.LogWarning($"Payment promise {transferInfo.PromiseId} not found for transfer {transfer}");
                            continue;
                        }

                        var validationRequest = new ValidatePaymentPromiseRequest
                        {
                            PaymentPromiseId = transferInfo.PromiseId.Value,
                            AmountPaid = transferInfo.Amount,
                            PaymentDate = DateTime.UtcNow
                        };

                        var validationResult = await _paymentPromiseService.ValidatePaymentPromiseAsync(validationRequest);
                        if (!validationResult.Success)
                        {
                            _logger.LogWarning($"Validation failed for promise {transferInfo.PromiseId}: {validationResult.Message}");
                            continue;
                        }

                        _logger.LogInformation($"Successfully validated payment for promise {transferInfo.PromiseId}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error processing transfer {transfer}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking emails");
            }
        }
    }
} 