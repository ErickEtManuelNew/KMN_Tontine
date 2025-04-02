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
        public decimal AmountPromised { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PromiseDate { get; set; }
        public DateTime? FulfilledDate { get; set; }
        public bool IsFulfilled { get; set; }
        public AccountType AccountName { get; set; }
    }
}
