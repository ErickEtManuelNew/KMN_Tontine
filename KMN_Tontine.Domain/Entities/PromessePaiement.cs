namespace KMN_Tontine.Domain.Entities
{
    public class PromessePaiement
    {
        public int Id { get; set; }
        public string MembreId { get; set; }
        public int CompteId { get; set; }
        public decimal Montant { get; set; }
        public string CodePaiement { get; set; }
        public string Statut { get; set; } = "En attente";
        public DateTime DatePromesse { get; set; } = DateTime.UtcNow;

        public Membre Membre { get; set; }
        public Compte Compte { get; set; }
    }
}
