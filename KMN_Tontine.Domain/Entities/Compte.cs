using KMN_Tontine.Domain.Enums;

namespace KMN_Tontine.Domain.Entities
{
    public class Compte
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public decimal Solde { get; set; }
        public string? Notes { get; set; }
        public string MembreId { get; set; } = string.Empty;
        public TypeCompte Type { get; set; } // 🔥 Enum TypeCompte
        public int AssociationId { get; set; }
        public Association Association { get; set; }

        public ICollection<MembreCompte> MembreComptes { get; set; } = new List<MembreCompte>();
    }
}
