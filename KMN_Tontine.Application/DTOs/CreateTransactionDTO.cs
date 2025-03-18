using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMN_Tontine.Application.DTOs
{
    public class CreateTransactionDTO
    {
        public string MembreId { get; set; }
        public int CompteId { get; set; }
        public decimal Montant { get; set; }
        public string TypeTransaction { get; set; } // "Crédit" ou "Débit"
    }
}
