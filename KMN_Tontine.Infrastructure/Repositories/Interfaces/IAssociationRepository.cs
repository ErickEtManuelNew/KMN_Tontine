using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Domain.Entities;

namespace KMN_Tontine.Infrastructure.Repositories.Interfaces
{
    public interface IAssociationRepository : IGenericRepository<Association>
    {
        Task<Association?> GetByIdAsync(int id);
    }
}
