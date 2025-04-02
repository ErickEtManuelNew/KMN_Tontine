using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Shared.Enums;

namespace KMN_Tontine.Shared.DTOs.Responses
{
    public class AccountResponse
    {
        public int Id { get; set; }
        public AccountType Type { get; set; }
        public decimal Balance { get; set; }
        public string Comment { get; set; }
        public string MemberFullName { get; set; }
        public int TontineId { get; set; }
    }
}
