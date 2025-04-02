namespace KMN_Tontine.Shared.DTOs.Requests
{
    public class CreatePaymentPromiseRequest
    {
        public decimal AmountPromised { get; set; }
        public DateTime PromiseDate { get; set; }
        public string MemberId { get; set; }
        public int AccountId { get; set; }
    }
}
