using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMN_Tontine.Domain.Entities
{
    public class PaymentPromise
    {
        public int Id { get; set; }
        public decimal AmountPromised { get; set; }
        public DateTime PromiseDate { get; set; } = DateTime.UtcNow;
        public DateTime? FulfilledDate { get; set; }
        public bool IsFulfilled => FulfilledDate.HasValue;

        public string MemberId { get; set; }
        public Member Member { get; set; }
    }
}
