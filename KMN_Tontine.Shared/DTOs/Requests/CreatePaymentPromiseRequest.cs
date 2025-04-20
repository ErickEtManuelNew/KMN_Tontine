using System;
using System.Collections.Generic;

namespace KMN_Tontine.Shared.DTOs.Requests
{
    public class CreatePaymentPromiseRequest
    {
        public DateTime PromiseDate { get; set; }
        public string MemberId { get; set; }
        public string Reference { get; set; }
        public List<CreatePaymentPromiseAccountRequest> Accounts { get; set; } = new();
    }

    public class CreatePaymentPromiseAccountRequest
    {
        public int AccountId { get; set; }
        public decimal AmountPromised { get; set; }
    }
}
