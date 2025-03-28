using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KMN_Tontine.Domain.Enums
{
    public enum AccountType
    {
        [EnumMember(Value = "Private")]
        Private = 0,
        [EnumMember(Value = "Help")]
        Help = 1,
        [EnumMember(Value = "Late")]
        Late = 2,
        [EnumMember(Value = "Other")]
        Other = 3
    }
}
