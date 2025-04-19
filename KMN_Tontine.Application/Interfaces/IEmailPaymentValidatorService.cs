using System.Threading.Tasks;

namespace KMN_Tontine.Application.Interfaces
{
    public interface IEmailPaymentValidatorService
    {
        Task CheckEmailsAsync();
    }
} 