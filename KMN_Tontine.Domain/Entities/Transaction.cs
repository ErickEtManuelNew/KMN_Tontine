using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using KMN_Tontine.Shared.Enums;

namespace KMN_Tontine.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int AccountId { get; set; }

        [Required]
        public virtual Account Account { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        public string Description { get; set; }

        public int TontineId { get; set; }
        public Tontine Tontine { get; set; }

        public int? PaymentPromiseId { get; set; }
        public virtual PaymentPromise? PaymentPromise { get; set; }
    }
}
