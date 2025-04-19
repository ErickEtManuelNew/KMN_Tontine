using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using KMN_Tontine.Application.Common;
using KMN_Tontine.Application.DTOs.Requests;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Shared.Enums;
using KMN_Tontine.Domain.Interfaces;
using KMN_Tontine.Shared.DTOs.Requests;
using KMN_Tontine.Shared.DTOs.Responses;
using KMN_Tontine.Infrastructure.Interface;
using KMN_Tontine.Infrastructure.Repositories.Implementations;

namespace KMN_Tontine.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper, IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionResponse> GetTransactionByIdAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null)
                throw new KeyNotFoundException("Transaction not found");

            return _mapper.Map<TransactionResponse>(transaction);
        }

        public async Task<IEnumerable<TransactionResponse>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TransactionResponse>>(transactions);
        }

        public async Task<SimpleResponse> CreateTransactionAsync(CreateTransactionRequest request)
        {
            try
            {
                var account = await _accountRepository.GetByIdAsync(request.AccountId);

                if (account == null)
                    return new SimpleResponse { Success = false, Message = "Compte introuvable." };

                if (request.Type == TransactionType.Retrait && account.Balance < request.Amount)
                    return new SimpleResponse { Success = false, Message = "Solde insuffisant pour cette opération." };

                var transaction = _mapper.Map<Transaction>(request);
                transaction.CreatedDate = DateTime.UtcNow;

                // Mise à jour du solde
                if (request.Type == TransactionType.Credit)
                    account.Balance += request.Amount;
                else
                    account.Balance -= request.Amount;

                await _transactionRepository.AddAsync(transaction);
                await _unitOfWork.SaveChangesAsync();

                return new SimpleResponse { Success = true, Message = "Transaction enregistrée avec succès." };
            }
            catch (Exception ex)
            {
                return SimpleResponse.Error($"Failed to create transaction: {ex.Message}");
            }
        }

        public async Task<SimpleResponse> UpdateTransactionAsync(int id, UpdateTransactionRequest request)
        {
            try
            {
                var transaction = await _transactionRepository.GetByIdAsync(id);
                if (transaction == null)
                    return SimpleResponse.Error("Transaction not found");

                // Mettre à jour les propriétés
                transaction.Description = request.Description ?? transaction.Description;

                await _transactionRepository.UpdateAsync(transaction);
                return SimpleResponse.Ok("Transaction updated successfully");
            }
            catch (Exception ex)
            {
                return SimpleResponse.Error($"Failed to update transaction: {ex.Message}");
            }
        }

        public async Task<SimpleResponse> DeleteTransactionAsync(int id)
        {
            try
            {
                var transaction = await _transactionRepository.GetByIdAsync(id);
                if (transaction == null)
                    return SimpleResponse.Error("Transaction not found");

                await _transactionRepository.DeleteAsync(id);
                return SimpleResponse.Ok("Transaction deleted successfully");
            }
            catch (Exception ex)
            {
                return SimpleResponse.Error($"Failed to delete transaction: {ex.Message}");
            }
        }

        public async Task<List<TransactionResponse>> GetTransactionsByAccountIdAsync(int accountId)
        {
            var transactions = await _transactionRepository.GetByAccountIdAsync(accountId);
            return _mapper.Map<List<TransactionResponse>>(transactions);
        }

        public async Task<List<TransactionResponse>> GetTransactionsByMemberIdAsync(string memberId)
        {
            var transactions = await _transactionRepository.GetByMemberIdAsync(memberId);
            return _mapper.Map<List<TransactionResponse>>(transactions);
        }
    }
}
