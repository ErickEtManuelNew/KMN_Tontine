using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Domain.Enums;

namespace KMN_Tontine.Application.DTOs.Responses
{
    public class MemberResponse
    {
        public string Id { get; set; } // Identifiant unique hérité de IdentityUser
        public string FullName { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public RoleType Role { get; set; }
        public string Email { get; set; } // Hérité de IdentityUser
    }
}
