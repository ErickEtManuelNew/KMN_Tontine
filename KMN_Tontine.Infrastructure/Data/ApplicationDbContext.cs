using System.Reflection.Emit;

using KMN_Tontine.Domain.Entities;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KMN_Tontine.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<Member>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Tontine> Tontines { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PaymentPromise> PaymentPromises { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Account>().Property(a => a.Type).HasConversion<string>();
            builder.Entity<Transaction>().Property(t => t.Type).HasConversion<string>();

            builder.Entity<PaymentPromise>()
                .Property(p => p.AmountPromised)
                .HasColumnType("decimal(18,2)");
        }
    }
}
