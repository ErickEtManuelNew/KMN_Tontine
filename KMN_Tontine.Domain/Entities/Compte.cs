using KMN_Tontine.Domain.Enums;

namespace KMN_Tontine.Domain.Entities
{
    public class Compte
    {
        public int Id { get; set; }
        public TypeCompte Type { get; set; } // 🔥 Enum TypeCompte
        public decimal Solde { get; set; } = 0;
        public int AssociationId { get; set; }
        public Association Association { get; set; }

        public ICollection<MembreCompte> MembreComptes { get; set; } = new List<MembreCompte>();

    }
}
