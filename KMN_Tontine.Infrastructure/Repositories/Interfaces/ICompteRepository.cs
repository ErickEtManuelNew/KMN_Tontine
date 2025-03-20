using KMN_Tontine.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KMN_Tontine.Infrastructure.Repositories.Interfaces
{
    public interface ICompteRepository
    {
        Task<IEnumerable<Compte>> GetComptesByMembreIdAsync(string membreId);
        Task<Compte?> GetByIdAsync(int id);
        Task<Compte> AddAsync(Compte compte);
        Task UpdateAsync(Compte compte);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 