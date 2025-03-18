using AutoMapper;
using KMN_Tontine.Application.DTOs;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Domain.Enums;
using KMN_Tontine.Infrastructure.Data;
using KMN_Tontine.Infrastructure.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMN_Tontine.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(string memberId)
        {
            return await _transactionRepository.GetByMembreIdAsync(memberId);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByStatus(TransactionStatus status)
        {
            return await _transactionRepository.GetByStatusAsync(status);
        }

        public async Task AjouterTransactionAsync(string membreId, int compteId, decimal montant, TypeTransaction type)
        {
            var transaction = new Transaction
            {
                MembreId = membreId,
                CompteId = compteId,
                Montant = montant,
                Type = type,
                Status = TransactionStatus.EnAttente
            };

            await _transactionRepository.AddAsync(transaction);
        }
    }
}
