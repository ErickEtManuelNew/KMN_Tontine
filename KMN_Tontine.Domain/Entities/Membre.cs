
using KMN_Tontine.Domain.Enums;

using Microsoft.AspNetCore.Identity;

namespace KMN_Tontine.Domain.Entities
{
    public class Membre : IdentityUser
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public TypeMembre Type { get; set; } // 🔥 Enum TypeMembre
        public decimal SoldeComptePrive { get; set; } = 0;
        public int AssociationId { get; set; }
        public Association Association { get; set; }
        public ICollection<MembreCompte> MembreComptes { get; set; } = new List<MembreCompte>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}
