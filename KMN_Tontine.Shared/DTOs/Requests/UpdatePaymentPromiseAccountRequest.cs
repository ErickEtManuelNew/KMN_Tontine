using KMN_Tontine.Shared.Enums;

namespace KMN_Tontine.Shared.DTOs.Requests
{
    public class UpdatePaymentPromiseAccountRequest
    {
        public int AccountId { get; set; }
        public AccountType AccountName { get; set; }
        public decimal AmountPromised { get; set; }
    }
}
