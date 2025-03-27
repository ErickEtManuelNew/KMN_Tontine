using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using KMN_Tontine.Application.Common;
using KMN_Tontine.Application.DTOs.Requests;
using KMN_Tontine.Application.DTOs.Responses;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Domain.Interfaces;

namespace KMN_Tontine.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
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
                var transaction = _mapper.Map<Transaction>(request);
                transaction.Date = DateTime.UtcNow; // Définir la date de la transaction
                await _transactionRepository.AddAsync(transaction);

                return SimpleResponse.Ok("Transaction created successfully");
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
    }
}
