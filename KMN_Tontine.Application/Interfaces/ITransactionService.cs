using KMN_Tontine.Application.DTOs;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMN_Tontine.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetTransactionsByStatus(TransactionStatus status);
        Task<IEnumerable<Transaction>> GetTransactionsAsync(string membreId);
        Task AjouterTransactionAsync(string membreId, int compteId, decimal montant, TypeTransaction type);
    }
}
