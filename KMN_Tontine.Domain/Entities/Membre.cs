
using KMN_Tontine.Domain.Enums;

using Microsoft.AspNetCore.Identity;

namespace KMN_Tontine.Domain.Entities
{
    public class Membre : IdentityUser
    {
        public required string LastName { get; set; }
        public required string Firstname { get; set; }
        public TypeMembre Type { get; set; } // 🔥 Enum TypeMembre
        public decimal SoldeComptePrive { get; set; } = 0;
        public int AssociationId { get; set; }
        public Association? Association { get; set; }
        public ICollection<MembreCompte> MembreComptes { get; set; } = new List<MembreCompte>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}
