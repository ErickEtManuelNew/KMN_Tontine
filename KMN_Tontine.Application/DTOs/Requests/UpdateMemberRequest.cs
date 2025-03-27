using KMN_Tontine.Domain.Enums;

namespace KMN_Tontine.Application.DTOs.Requests
{
    public class UpdateMemberRequest
    {
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? IsActive { get; set; }
        public RoleType? Role { get; set; }
    }
}