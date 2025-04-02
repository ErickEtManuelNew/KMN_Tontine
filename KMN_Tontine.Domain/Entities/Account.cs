using KMN_Tontine.Shared.Enums;

namespace KMN_Tontine.Domain.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public AccountType Type { get; set; }
        public decimal Balance { get; set; }
        public string? Comment { get; set; }

        public string? MemberId { get; set; }
        public Member? Member { get; set; }

        public int TontineId { get; set; }
        public Tontine Tontine { get; set; }

        public bool IsAssociationAccount => MemberId == null;

        public ICollection<Transaction> Transactions { get; set; }
    }
}
