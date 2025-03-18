using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMN_Tontine.Domain.Entities
{
    public class Association
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Email { get; set; }
        public ICollection<Membre> Membres { get; set; } = new List<Membre>();
        public ICollection<Compte> Comptes { get; set; } = new List<Compte>();
    }
}
