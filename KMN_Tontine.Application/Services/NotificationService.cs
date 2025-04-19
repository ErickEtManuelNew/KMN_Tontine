using System;
using System.Threading.Tasks;
using KMN_Tontine.Application.Common;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Domain.Interfaces;

namespace KMN_Tontine.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IEmailService _emailService;

        public NotificationService(IMemberRepository memberRepository, IEmailService emailService)
        {
            _memberRepository = memberRepository;
            _emailService = emailService;
        }

        public async Task<SimpleResponse> SendPaymentReminderAsync(string memberId, decimal remainingAmount, int promiseId)
        {
            try
            {
                var member = await _memberRepository.GetByIdAsync(Guid.Parse(memberId));
                if (member == null)
                    return SimpleResponse.Error("Membre non trouvé");

                // Envoyer l'email de notification
                var emailSent = await _emailService.SendPaymentReminderAsync(
                    member.Email,
                    member.FullName,
                    remainingAmount,
                    promiseId
                );

                if (!emailSent)
                {
                    return SimpleResponse.Error("Erreur lors de l'envoi de la notification par email");
                }

                return SimpleResponse.Ok("Notification envoyée avec succès");
            }
            catch (Exception ex)
            {
                return SimpleResponse.Error($"Erreur lors de l'envoi de la notification : {ex.Message}");
            }
        }
    }
} 