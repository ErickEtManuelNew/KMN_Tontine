using KMN_Tontine.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMN_Tontine.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<List<TransactionDTO>> GetTransactionsAsync(string membreId);
        Task<TransactionDTO> CrediterAsync(CreateTransactionDTO transaction);
    }
}
