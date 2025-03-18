using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Domain.Enums;

namespace KMN_Tontine.Infrastructure.Repositories.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetByStatusAsync(TransactionStatus status);
        Task<IEnumerable<Transaction>> GetByMembreIdAsync(string membreId);
    }
}
