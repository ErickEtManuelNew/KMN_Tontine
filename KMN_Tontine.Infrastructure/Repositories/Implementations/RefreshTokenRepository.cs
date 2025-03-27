using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Domain.Interfaces;
using KMN_Tontine.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace KMN_Tontine.Infrastructure.Repositories.Implementations
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
            => await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);

        public async Task<IEnumerable<RefreshToken>?> GetByMemberIdAsync(Guid memberId)
            => await _context.RefreshTokens
                .Where(rt => rt.MemberId == memberId.ToString())
                .OrderByDescending(rt => rt.Expires)
                .ToListAsync();

        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(string token)
        {
            var entity = await GetByTokenAsync(token);
            if (entity is not null)
            {
                _context.RefreshTokens.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string token)
        {
            var existing = await GetByTokenAsync(token);
            if (existing is not null)
            {
                _context.RefreshTokens.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAllForUserAsync(Guid memberId)
        {
            var tokens = await _context.RefreshTokens
                .Where(rt => rt.MemberId == memberId.ToString())
                .ToListAsync();

            if (tokens.Any())
            {
                _context.RefreshTokens.RemoveRange(tokens);
                await _context.SaveChangesAsync();
            }
        }
    }
}
