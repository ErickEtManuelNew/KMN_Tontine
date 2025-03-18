namespace KMN_Tontine.Domain.Entities
{
	public class MembreCompte
	{
        public string MembreId { get; set; }
        public Membre Membre { get; set; }
        public int CompteId { get; set; }
        public Compte Compte { get; set; }
        public decimal Solde { get; set; } = 0;
    }
}
