using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMN_Tontine.Application.DTOs
{
    public class RegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Matricule { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string TypeMembre { get; set; }
    }
}
