using System;
using System.Collections.Generic;

namespace KMN_Tontine.Shared.DTOs.Requests
{
    public class UpdatePaymentPromiseRequest
    {
        public int Id { get; set; }
        public DateTime PromiseDate { get; set; }
        public List<UpdatePaymentPromiseAccountRequest> Accounts { get; set; } = new();
    }
}