using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Infrastructure.Repositories.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KMN_Tontine.Infrastructure.Repositories.Implementations
{
    public class MembreRepository : IMembreRepository
    {
        private readonly UserManager<Membre> _userManager;

        public MembreRepository(UserManager<Membre> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Membre?> GetByMembreIdAsync(string membreId)
        {
            return await _userManager.Users
                .Include(m => m.Association)
                .Include(m => m.MembreComptes)
                .FirstOrDefaultAsync(m => m.Id == membreId);
        }

        public async Task<bool> ExisteParEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        public async Task<Membre?> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task AjouterMembreAsync(Membre membre, string password)
        {
            var result = await _userManager.CreateAsync(membre, password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
