using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KMN_Tontine.Domain.Entities
{
    /// <summary>
    /// Représente une promesse de paiement faite par un membre.
    /// Une promesse peut être liée à plusieurs comptes, avec un montant spécifique pour chaque compte.
    /// </summary>
    public class PaymentPromise
    {
        public PaymentPromise()
        {
            PaymentPromiseAccounts = new HashSet<PaymentPromiseAccount>();
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime PromiseDate { get; set; } = DateTime.UtcNow;

        public DateTime? FulfilledDate { get; set; }

        public bool IsFulfilled => FulfilledDate.HasValue;

        [Required]
        [StringLength(7)]
        public string Reference { get; set; }

        [Required]
        [ForeignKey(nameof(Member))]
        public string MemberId { get; set; }

        [Required]
        public virtual Member Member { get; set; }

        /// <summary>
        /// Liste des comptes associés à cette promesse avec leurs montants respectifs
        /// </summary>
        public virtual ICollection<PaymentPromiseAccount> PaymentPromiseAccounts { get; set; }

        /// <summary>
        /// Liste des transactions associées à cette promesse
        /// </summary>
        public virtual ICollection<Transaction> Transactions { get; set; }

        /// <summary>
        /// Montant total de la promesse (somme des montants promis pour chaque compte)
        /// </summary>
        [NotMapped]
        public decimal TotalAmountPromised => CalculateTotalAmount();

        private decimal CalculateTotalAmount()
        {
            decimal total = 0;
            foreach (var promiseAccount in PaymentPromiseAccounts)
            {
                total += promiseAccount.AmountPromised;
            }
            return total;
        }
    }
}
