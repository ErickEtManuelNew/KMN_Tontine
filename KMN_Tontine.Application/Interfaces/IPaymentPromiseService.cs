using KMN_Tontine.Application.Common;
using KMN_Tontine.Shared.DTOs.Requests;
using KMN_Tontine.Shared.DTOs.Responses;

namespace KMN_Tontine.Application.Interfaces
{
    public interface IPaymentPromiseService
    {
        Task<PaymentPromiseResponse> GetPaymentPromiseByIdAsync(int id);
        Task<IEnumerable<PaymentPromiseResponse>> GetAllPaymentPromisesAsync();
        Task<SimpleResponse> CreatePaymentPromiseAsync(CreatePaymentPromiseRequest request);
        Task<SimpleResponse> UpdatePaymentPromiseAsync(int id, UpdatePaymentPromiseRequest request);
        Task<SimpleResponse> DeletePaymentPromiseAsync(int id);
        Task<List<PaymentPromiseResponse>> GetByAccountIdAsync(int accountId);
        Task<List<PaymentPromiseResponse>> GetByMemberIdAsync(Guid memberId);
        Task<SimpleResponse> ValidatePaymentPromiseAsync(ValidatePaymentPromiseRequest request);
    }
}
