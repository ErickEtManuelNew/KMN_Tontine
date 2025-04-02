using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Application.Common;
using KMN_Tontine.Shared.DTOs.Requests;
using KMN_Tontine.Shared.DTOs.Responses;

namespace KMN_Tontine.Application.Interfaces
{
    public interface IAccountService
    {
        Task<AccountResponse> GetAccountByIdAsync(int id);
        Task<IEnumerable<AccountResponse>> GetAllAccountsAsync();
        Task<IEnumerable<AccountResponse>> GetAccountsByMemberIdAsync(Guid memberid);
        Task<SimpleResponse> CreateAccountAsync(CreateAccountRequest request);
        Task<SimpleResponse> CreateAccountForMemberAsync(string memberId);
        Task<SimpleResponse> UpdateAccountAsync(int id, UpdateAccountRequest request);
        Task<SimpleResponse> DeleteAccountAsync(int id);
    }
}
