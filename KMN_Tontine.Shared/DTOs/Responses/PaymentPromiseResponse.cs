using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Shared.Enums;

namespace KMN_Tontine.Shared.DTOs.Responses
{
    public class PaymentPromiseResponse
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PromiseDate { get; set; }
        public DateTime? FulfilledDate { get; set; }
        public bool IsFulfilled { get; set; }
        public List<PaymentPromiseAccountResponse> Accounts { get; set; } = new();
        public decimal TotalAmountPromised { get; set; }
    }

    public class PaymentPromiseAccountResponse
    {
        public int AccountId { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountName { get; set; }
        public decimal AmountPromised { get; set; }
    }
}
