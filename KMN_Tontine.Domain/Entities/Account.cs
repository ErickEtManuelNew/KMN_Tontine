using System.Collections.Generic;
using KMN_Tontine.Shared.Enums;

namespace KMN_Tontine.Domain.Entities
{
    public class Account
    {
        public Account()
        {
            Transactions = new HashSet<Transaction>();
            PaymentPromiseAccounts = new HashSet<PaymentPromiseAccount>();
        }

        public int Id { get; set; }
        public AccountType Type { get; set; }
        public decimal Balance { get; set; }
        public string? Comment { get; set; }

        public string? MemberId { get; set; }
        public virtual Member? Member { get; set; }

        public int TontineId { get; set; }
        public virtual Tontine Tontine { get; set; }

        public bool IsAssociationAccount => MemberId == null;

        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<PaymentPromiseAccount> PaymentPromiseAccounts { get; set; }
    }
}
