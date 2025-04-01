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
    public class MemberRepository : IMemberRepository
    {
        private readonly ApplicationDbContext _context;

        public MemberRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Member?> GetByIdAsync(Guid id)
        {
            return await _context.Members.FindAsync(id);
        }

        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await _context.Members
                .Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task AddAsync(Member member)
        {
            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Member member)
        {
            _context.Members.Update(member);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Member?> GetByEmailAsync(string email)
        {
            return await _context.Members
                .FirstOrDefaultAsync(m => m.Email == email);
        }
    }
}
