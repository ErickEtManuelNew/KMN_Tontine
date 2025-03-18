using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMN_Tontine.Application.DTOs
{
    public class CompteDTO
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public bool EstComptePrive { get; set; }
        public decimal Solde { get; set; }
    }
}
