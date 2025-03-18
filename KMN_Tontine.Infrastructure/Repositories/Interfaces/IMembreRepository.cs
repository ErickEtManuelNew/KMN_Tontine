using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Domain.Entities;

namespace KMN_Tontine.Infrastructure.Repositories.Interfaces
{
    public interface IMembreRepository
    {
        Task<Membre?> GetByMembreIdAsync(string membreId);
        Task<bool> ExisteParEmailAsync(string email);
        Task<Membre?> GetByEmailAsync(string email);
        Task AjouterMembreAsync(Membre membre, string password);
    }
}
