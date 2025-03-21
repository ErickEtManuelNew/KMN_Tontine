﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Domain.Enums;

namespace KMN_Tontine.Application.DTOs
{
    public class InscriptionMembreDto
    {
        public string Nom { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int AssociationId { get; set; }
        public TypeMembre Type { get; set; }
    }
}
