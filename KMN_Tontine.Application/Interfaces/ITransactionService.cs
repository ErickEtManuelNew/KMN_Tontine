using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Application.Common;
using KMN_Tontine.Application.DTOs.Requests;
using KMN_Tontine.Application.DTOs.Responses;

namespace KMN_Tontine.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionResponse> GetTransactionByIdAsync(int id);
        Task<IEnumerable<TransactionResponse>> GetAllTransactionsAsync();
        Task<SimpleResponse> CreateTransactionAsync(CreateTransactionRequest request);
        Task<SimpleResponse> UpdateTransactionAsync(int id, UpdateTransactionRequest request);
        Task<SimpleResponse> DeleteTransactionAsync(int id);
    }
}
