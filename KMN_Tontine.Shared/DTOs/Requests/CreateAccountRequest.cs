using KMN_Tontine.Shared.Enums;

namespace KMN_Tontine.Shared.DTOs.Requests
{
    public class CreateAccountRequest
    {
        public AccountType Type { get; set; }
        public decimal InitialBalance { get; set; }
        public string MemberId { get; set; }
        public int TontineId { get; set; }
    }
}