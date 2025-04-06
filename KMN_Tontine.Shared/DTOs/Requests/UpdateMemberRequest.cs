using KMN_Tontine.Shared.Enums;

namespace KMN_Tontine.Shared.DTOs.Requests
{
    public class UpdateMemberRequest
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? IsActive { get; set; }
        public RoleType Role { get; set; }
        public bool? LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}