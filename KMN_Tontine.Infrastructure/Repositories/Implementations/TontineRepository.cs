using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Domain.Interfaces;
using KMN_Tontine.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace KMN_Tontine.Infrastructure.Repositories.Implementations
{
    public class TontineRepository : ITontineRepository
    {
        private readonly ApplicationDbContext _context;

        public TontineRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tontine>> GetAllAsync()
            => await _context.Tontines.ToListAsync();

        public async Task<Tontine?> GetByIdAsync(int id)
            => await _context.Tontines.FindAsync(id);

        public async Task AddAsync(Tontine tontine)
        {
            await _context.Tontines.AddAsync(tontine);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tontine tontine)
        {
            _context.Tontines.Update(tontine);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tontine = await _context.Tontines.FindAsync(id);
            if (tontine is not null)
            {
                _context.Tontines.Remove(tontine);
                await _context.SaveChangesAsync();
            }
        }
    }
}
