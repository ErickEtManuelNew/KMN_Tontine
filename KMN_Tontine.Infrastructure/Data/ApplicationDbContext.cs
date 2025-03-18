using KMN_Tontine.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KMN_Tontine.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<Membre>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Compte> Comptes { get; set; }
        public DbSet<MembreCompte> MembreComptes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PromessePaiement> PromessesPaiement { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Définition de la précision pour éviter les erreurs de troncature
            builder.Entity<Compte>()
                .Property(c => c.Solde)
                .HasPrecision(18, 2);

            builder.Entity<Membre>()
                .Property(m => m.SoldeComptePrive)
                .HasPrecision(18, 2);

            builder.Entity<MembreCompte>()
                .Property(mc => mc.Solde)
                .HasPrecision(18, 2);

            builder.Entity<Transaction>()
                .Property(t => t.Montant)
                .HasPrecision(18, 2);

            builder.Entity<PromessePaiement>()
                .Property(t => t.Montant)
                .HasPrecision(18, 2);

            // Relation Membre ↔ Compte (clé composite)
            builder.Entity<MembreCompte>()
                .HasKey(mc => new { mc.MembreId, mc.CompteId });

            builder.Entity<MembreCompte>()
                .HasOne(mc => mc.Membre)
                .WithMany(m => m.MembreComptes)
                .HasForeignKey(mc => mc.MembreId);

            builder.Entity<MembreCompte>()
                .HasOne(mc => mc.Compte)
                .WithMany(c => c.MembreComptes)
                .HasForeignKey(mc => mc.CompteId);
        }
    }
}
