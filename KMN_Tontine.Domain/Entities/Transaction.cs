using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Domain.Enums;

namespace KMN_Tontine.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Description { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int TontineId { get; set; }
        public Tontine Tontine { get; set; }
    }
}
