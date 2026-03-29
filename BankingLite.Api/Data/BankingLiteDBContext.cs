using Banking.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingLite.Api.Data
{
    public class BankingLiteDbContext : DbContext
    {
        public BankingLiteDbContext(DbContextOptions<BankingLiteDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.FullName).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.PasswordSalt).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
