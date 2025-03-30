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
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public decimal AmountPromised { get; set; }
        public DateTime PromiseDate { get; set; } = DateTime.UtcNow;
        public DateTime? FulfilledDate { get; set; }
        public bool IsFulfilled => FulfilledDate.HasValue;

        public int AccountId { get; set; }
        public Account Account { get; set; }
        public string MemberId { get; set; }
        public Member Member { get; set; }
    }
}
