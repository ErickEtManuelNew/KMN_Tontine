namespace KMN_Tontine.Application.DTOs.Requests
{
    public class UpdatePaymentPromiseRequest
    {
        public int Id { get; set; }
        public DateTime PromiseDate { get; set; }
        public decimal AmountPromised { get; set; } // Changed from CreatePaymentPromiseRequest.
    }
}