﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KMN_Tontine.Domain.Enums
{
    public enum AccountType
    {
        [EnumMember(Value = "Privé")]
        Privé = 0,
        [EnumMember(Value = "Aide")]
        Aide = 1,
        [EnumMember(Value = "Retard")]
        Retard = 2,
        [EnumMember(Value = "Autre")]
        Autre = 3
    }
}
