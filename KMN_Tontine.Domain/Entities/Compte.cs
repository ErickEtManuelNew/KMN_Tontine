namespace KMN_Tontine.Domain.Entities
{
    public class Compte
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public bool EstComptePrive { get; set; } = false;
        public decimal Solde { get; set; } = 0;

        public List<MembreCompte> MembreComptes { get; set; } = new();
    }
}
