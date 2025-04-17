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
        public DbSet<PaymentPromiseAccount> PaymentPromiseAccounts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Account>().Property(a => a.Type).HasConversion<string>();
            builder.Entity<Transaction>().Property(t => t.Type).HasConversion<string>();

            builder.Entity<PaymentPromise>(entity =>
            {
                entity.HasOne(p => p.Member)
                      .WithMany(m => m.PaymentPromises)
                      .HasForeignKey(p => p.MemberId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.PaymentPromiseAccounts)
                      .WithOne(pa => pa.PaymentPromise)
                      .HasForeignKey(pa => pa.PaymentPromiseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<PaymentPromiseAccount>(entity =>
            {
                entity.Property(pa => pa.AmountPromised)
                      .HasColumnType("decimal(18,2)");

                entity.HasOne(pa => pa.Account)
                      .WithMany(a => a.PaymentPromiseAccounts)
                      .HasForeignKey(pa => pa.AccountId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
