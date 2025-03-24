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
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Membre> Membres { get; set; } = new List<Membre>();
        public ICollection<Compte> Comptes { get; set; } = new List<Compte>();
    }
}
