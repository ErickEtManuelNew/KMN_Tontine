using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KMN_Tontine.Domain.Entities
{
    /// <summary>
    /// Représente la relation entre une promesse de paiement et un compte,
    /// avec le montant promis pour ce compte spécifique.
    /// </summary>
    public class PaymentPromiseAccount
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(PaymentPromise))]
        public int PaymentPromiseId { get; set; }

        [Required]
        public virtual PaymentPromise PaymentPromise { get; set; }

        [Required]
        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }

        [Required]
        public virtual Account Account { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPromised { get; set; }
    }
} 