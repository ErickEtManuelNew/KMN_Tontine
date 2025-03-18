using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Infrastructure.Data;
using KMN_Tontine.Infrastructure.Repositories.Interfaces;

namespace KMN_Tontine.Infrastructure.Repositories.Implementations
{
    public class AssociationRepository : GenericRepository<Association>, IAssociationRepository
    {
        public AssociationRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Association?> GetByIdAsync(int id)
        {
            return null; // _context.Associations.FindAsync(id);
        }
    }
}
