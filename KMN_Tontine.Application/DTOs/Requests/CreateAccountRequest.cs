using KMN_Tontine.Domain.Enums;

namespace KMN_Tontine.Application.DTOs.Requests
{
    public class CreateAccountRequest
    {
        public AccountType Type { get; set; }
        public decimal InitialBalance { get; set; }
        public string MemberId { get; set; }
        public int TontineId { get; set; }
    }
}