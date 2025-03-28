using KMN_Tontine.Domain.Enums;

using Microsoft.AspNetCore.Identity;

namespace KMN_Tontine.Domain.Entities
{
    public class Member : IdentityUser
    {
        /// <summary>
        /// Le prénom du membre.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Le nom de famille du membre.
        /// </summary>
        public string LastName { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; } = true;

        // 👇 Nouveau champ RoleType
        public RoleType Role { get; set; } = RoleType.Member;

        public ICollection<Account> Accounts { get; set; } = [];
        public ICollection<PaymentPromise> PaymentPromises { get; set; } = [];
        public string ConfirmationCode { get; set; }
    }
}
