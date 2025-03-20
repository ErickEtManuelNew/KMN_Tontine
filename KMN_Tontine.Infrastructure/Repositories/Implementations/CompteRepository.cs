using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Infrastructure.Data;
using KMN_Tontine.Infrastructure.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KMN_Tontine.Infrastructure.Repositories.Implementations
{
    public class CompteRepository : ICompteRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompteRepository> _logger;

        public CompteRepository(ApplicationDbContext context, ILogger<CompteRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Compte>> GetComptesByMembreIdAsync(string membreId)
        {
            try
            {
                var comptes = await _context.MembreComptes
                    .Where(mc => mc.MembreId == membreId)  // On filtre les comptes du membre
                    .Select(mc => mc.Compte)
                    .AsNoTracking()
                    .ToListAsync();

                return comptes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des comptes pour le membre {MembreId}", membreId);
                throw new ApplicationException($"Impossible de récupérer les comptes du membre {membreId}", ex);
            }
        }

        public async Task<Compte?> GetByIdAsync(int id)
        {
            return await _context.Comptes.FindAsync(id);
        }

        public async Task<Compte> AddAsync(Compte compte)
        {
            _context.Comptes.Add(compte);
            await _context.SaveChangesAsync();
            return compte;
        }

        public async Task UpdateAsync(Compte compte)
        {
            _context.Entry(compte).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var compte = await _context.Comptes.FindAsync(id);
            if (compte != null)
            {
                _context.Comptes.Remove(compte);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Comptes.AnyAsync(c => c.Id == id);
        }
    }
} 