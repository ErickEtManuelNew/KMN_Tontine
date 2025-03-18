namespace KMN_Tontine.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string MembreId { get; set; }
        public int CompteId { get; set; }
        public decimal Montant { get; set; }
        public DateTime DateTransaction { get; set; } = DateTime.UtcNow;
        public string TypeTransaction { get; set; } // "Crédit" ou "Débit"
        public string Justificatif { get; set; }
        public string NumeroFacture { get; set; }

        public Membre Membre { get; set; }
        public Compte Compte { get; set; }
    }
}
