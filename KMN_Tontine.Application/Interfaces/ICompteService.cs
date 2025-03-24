using KMN_Tontine.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KMN_Tontine.Application.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<CompteDTO>> GetComptesByMembreIdAsync(string membreId);
        Task<CompteDTO?> GetCompteByIdAsync(int id);
        Task<CompteDTO> CreateCompteAsync(CompteDTO compteDto);
        Task UpdateCompteAsync(CompteDTO compteDto);
        Task DeleteCompteAsync(int id);
    }
}
