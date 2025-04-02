using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Domain.Interfaces;
using KMN_Tontine.Infrastructure.Data;
using KMN_Tontine.Shared.DTOs.Responses;
using KMN_Tontine.Shared.Enums;

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
            return await _context.Members.FindAsync(id.ToString());
        }

        public async Task<IEnumerable<MemberResponse>> GetAllAsync()
        {
            return await _context.Members
            .Where(x => x.IsActive)
            .Select(user => new MemberResponse
            {
                Id = Guid.Parse(user.Id),
                FullName = user.FullName,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                EmailConfirmed = user.EmailConfirmed,
                // Requête EF pour obtenir les noms des rôles associés
                Role = Enum.Parse<RoleType>(_context.UserRoles
                            .Where(ur => ur.UserId == user.Id)
                            .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                            .FirstOrDefault()),
                IsActive = user.IsActive,
                JoinDate = user.JoinDate

            })
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
