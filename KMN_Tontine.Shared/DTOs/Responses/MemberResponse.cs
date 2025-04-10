﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Shared.Enums;

namespace KMN_Tontine.Shared.DTOs.Responses
{
    public class MemberResponse
    {
        public Guid Id { get; set; } // Identifiant unique hérité de IdentityUser
        public string FullName { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public RoleType Role { get; set; }
        public string Email { get; set; } // Hérité de IdentityUser
        public bool EmailConfirmed { get; set; } // Hérité de IdentityUser
        public bool LockoutEnabled { get; set; } // Hérité de IdentityUser
    }
}
