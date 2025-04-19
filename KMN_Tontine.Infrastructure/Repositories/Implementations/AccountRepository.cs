using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Domain.Interfaces;
using KMN_Tontine.Infrastructure.Data;
using KMN_Tontine.Shared.Enums;

using Microsoft.EntityFrameworkCore;

namespace KMN_Tontine.Infrastructure.Repositories.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
            => await _context.Accounts
            .Include(x => x.Member)
            .ToListAsync();

        public async Task<List<Account>> GetByMemberIdAsync(string memberId)
            => await _context.Accounts
                .Where(a => a.MemberId == memberId)
                .ToListAsync();

        public async Task<Account?> GetByIdAsync(int id)
            => await _context.Accounts.FindAsync(id);

        public async Task<Account?> GetPrivateAccountAsync(string memberId)
            => await _context.Accounts
                .FirstOrDefaultAsync(a => a.MemberId == memberId && a.Type == AccountType.Prive);

        public async Task AddAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account is not null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }
    }
}
