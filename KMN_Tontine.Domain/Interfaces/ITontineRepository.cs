using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Domain.Entities;

namespace KMN_Tontine.Domain.Interfaces
{
    public interface ITontineRepository
    {
        Task<Tontine?> GetByIdAsync(int id);
        Task<IEnumerable<Tontine>> GetAllAsync();
        Task AddAsync(Tontine tontine);
        Task UpdateAsync(Tontine tontine);
        Task DeleteAsync(int id);
    }
}
