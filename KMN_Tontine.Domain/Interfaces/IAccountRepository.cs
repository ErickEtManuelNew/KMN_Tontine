using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Domain.Entities;

namespace KMN_Tontine.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(int id);
        Task<List<Account>> GetByMemberIdAsync(string? memberId);
        Task<Account?> GetPrivateAccountAsync(string memberId);
        Task<IEnumerable<Account>> GetAllAsync();
        Task AddAsync(Account account);
        Task UpdateAsync(Account account);
        Task DeleteAsync(int id);
    }
}
