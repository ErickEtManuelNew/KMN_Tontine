using KMN_Tontine.Domain.Enums;

namespace KMN_Tontine.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string MembreId { get; set; }
        public Membre Membre { get; set; }

        public int CompteId { get; set; } // 🔥 Ajout du compte lié à la transaction
        public Compte Compte { get; set; }

        public decimal Montant { get; set; }
        public DateTime DateTransaction { get; set; } = DateTime.UtcNow;
        public TransactionStatus Status { get; set; } = TransactionStatus.EnAttente; // 🔥 Enum TransactionStatus
        public TypeTransaction Type { get; set; } // 🔥 Enum TypeTransaction

        public string? CodeValidation { get; set; } // Code de validation (via email)
    }
}
