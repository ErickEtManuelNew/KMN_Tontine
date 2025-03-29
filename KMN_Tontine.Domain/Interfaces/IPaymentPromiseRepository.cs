using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Domain.Entities;

namespace KMN_Tontine.Domain.Interfaces
{
    public interface IPaymentPromiseRepository
    {
        Task<PaymentPromise?> GetByIdAsync(int id);
        Task<IEnumerable<PaymentPromise>> GetByMemberIdAsync(Guid memberId);
        Task<IEnumerable<PaymentPromise>> GetAllAsync(); 
        Task AddAsync(PaymentPromise promise);
        Task UpdateAsync(PaymentPromise promise);
        Task DeleteAsync(int id);
        Task<List<PaymentPromise>> GetByAccountIdAsync(int accountId);
    }
}
