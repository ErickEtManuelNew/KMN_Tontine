using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMN_Tontine.Application.DTOs.Requests
{
    public class CreatePaymentPromiseRequest
    {
        public decimal AmountPromised { get; set; }
        public DateTime PromiseDate { get; set; }
        public string MemberId { get; set; }
        public int AccountId { get; set; }
    }
}
