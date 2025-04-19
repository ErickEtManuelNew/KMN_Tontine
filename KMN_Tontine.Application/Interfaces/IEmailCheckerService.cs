using System.Collections.Generic;
using System.Threading.Tasks;
using KMN_Tontine.Shared.DTOs.Responses;

namespace KMN_Tontine.Application.Interfaces
{
    public interface IEmailCheckerService
    {
        Task<IEnumerable<string>> GetTransfersAsync();
        Task<TransferInfo> GetTransferDetailsAsync(string transferId);
        Task CheckEmailsAsync();
    }
} 