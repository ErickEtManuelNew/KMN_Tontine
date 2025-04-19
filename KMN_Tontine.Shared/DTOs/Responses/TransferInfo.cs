using System;

namespace KMN_Tontine.Shared.DTOs.Responses
{
    public class TransferInfo
    {
        public decimal Amount { get; set; }
        public string SenderName { get; set; }
        public int? PromiseId { get; set; }
    }
} 