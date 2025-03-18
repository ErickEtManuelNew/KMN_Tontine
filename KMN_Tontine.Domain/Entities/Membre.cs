
using Microsoft.AspNetCore.Identity;

namespace KMN_Tontine.Domain.Entities
{
    public class Membre : IdentityUser
    {
        public string Matricule { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string TypeMembre { get; set; } // Adhérent, Administrateur...
        public decimal SoldeComptePrive { get; set; } = 0; // Toujours créditeur

        // Propriété de navigation vers les comptes d'opération
        public List<MembreCompte> MembreComptes { get; set; } = new();
    }
}
