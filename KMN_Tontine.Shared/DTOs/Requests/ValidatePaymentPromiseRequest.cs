using System;

namespace KMN_Tontine.Shared.DTOs.Requests
{
    public class ValidatePaymentPromiseRequest
    {
        public int PaymentPromiseId { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
} 