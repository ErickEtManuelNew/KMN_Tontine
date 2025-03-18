using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Domain.Enums;
using KMN_Tontine.Infrastructure.Data;
using KMN_Tontine.Infrastructure.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace KMN_Tontine.Infrastructure.Repositories.Implementations
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Transaction>> GetByStatusAsync(TransactionStatus status)
        {
            return await _context.Transactions.Where(t => t.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByMembreIdAsync(string membreId)
        {
            return await _context.Transactions.Where(t => t.MembreId == membreId).ToListAsync();
        }
    }
}
