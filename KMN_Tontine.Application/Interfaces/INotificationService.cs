using System;
using System.Threading.Tasks;
using KMN_Tontine.Application.Common;

namespace KMN_Tontine.Application.Interfaces
{
    public interface INotificationService
    {
        Task<SimpleResponse> SendPaymentReminderAsync(string memberId, decimal remainingAmount, string promiseReference);
    }
} 