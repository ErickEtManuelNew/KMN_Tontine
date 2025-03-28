using AutoMapper;

using KMN_Tontine.Application.Common;
using KMN_Tontine.Application.DTOs.Requests;
using KMN_Tontine.Application.DTOs.Responses;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Domain.Enums;
using KMN_Tontine.Domain.Interfaces;
using KMN_Tontine.Infrastructure.Repositories.Implementations;

namespace KMN_Tontine.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<AccountResponse> GetAccountByIdAsync(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
                throw new KeyNotFoundException("Account not found");

            return _mapper.Map<AccountResponse>(account);
        }

        public async Task<IEnumerable<AccountResponse>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AccountResponse>>(accounts);
        }

        public async Task<IEnumerable<AccountResponse>> GetAccountsByMemberIdAsync(Guid memberid)
        {
            var accounts = await _accountRepository.GetByMemberIdAsync(memberid);
            return _mapper.Map<IEnumerable<AccountResponse>>(accounts);
        }

        public async Task<SimpleResponse> CreateAccountAsync(CreateAccountRequest request)
        {
            try
            {
                var account = _mapper.Map<Account>(request);
                account.Balance = request.InitialBalance; // Initialiser le solde
                await _accountRepository.AddAsync(account);

                return SimpleResponse.Ok("Account created successfully");
            }
            catch (Exception ex)
            {
                return SimpleResponse.Error($"Failed to create account: {ex.Message}");
            }
        }

        public async Task<SimpleResponse> UpdateAccountAsync(int id, UpdateAccountRequest request)
        {
            try
            {
                var account = await _accountRepository.GetByIdAsync(id);
                if (account == null)
                    return SimpleResponse.Error("Account not found");

                // Mettre à jour les propriétés
                account.Balance = request.Balance ?? account.Balance;

                await _accountRepository.UpdateAsync(account);
                return SimpleResponse.Ok("Account updated successfully");
            }
            catch (Exception ex)
            {
                return SimpleResponse.Error($"Failed to update account: {ex.Message}");
            }
        }

        public async Task<SimpleResponse> DeleteAccountAsync(int id)
        {
            try
            {
                var account = await _accountRepository.GetByIdAsync(id);
                if (account == null)
                    return SimpleResponse.Error("Account not found");

                await _accountRepository.DeleteAsync(id);
                return SimpleResponse.Ok("Account deleted successfully");
            }
            catch (Exception ex)
            {
                return SimpleResponse.Error($"Failed to delete account: {ex.Message}");
            }
        }

        public async Task<SimpleResponse> CreateAccountForMemberAsync(string memberId)
        {
            // Créer les comptes associés à l'utilisateur
            // Affecter tous les types de comptes (enum) à l'utilisateur
            foreach (AccountType accountType in Enum.GetValues<AccountType>())
            {
                await _accountRepository.AddAsync(new Account
                {
                    MemberId = memberId,
                    Balance = 0,
                    Type = accountType,
                    TontineId = 1
                });
            }

            return await Task.FromResult(SimpleResponse.Ok("Accounts created successfully"));
        }
    }
}
