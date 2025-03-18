using AutoMapper;
using KMN_Tontine.Application.DTOs;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Infrastructure.Data;
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
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TransactionService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TransactionDTO>> GetTransactionsAsync(string membreId)
        {
            var transactions = await _context.Transactions
                .Where(t => t.MembreId == membreId)
                .ToListAsync();

            return _mapper.Map<List<TransactionDTO>>(transactions);
        }

        public async Task<TransactionDTO> CrediterAsync(CreateTransactionDTO dto)
        {
            var transaction = _mapper.Map<Transaction>(dto);
            transaction.DateTransaction = DateTime.UtcNow;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return _mapper.Map<TransactionDTO>(transaction);
        }
    }
}
