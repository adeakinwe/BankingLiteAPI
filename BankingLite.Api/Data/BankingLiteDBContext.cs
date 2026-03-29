using BankingLite.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingLite.Api.Data
{
    public class BankingLiteDbContext : DbContext
    {
        public BankingLiteDbContext(DbContextOptions<BankingLiteDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.FullName).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.PasswordSalt).IsRequired();
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.FullName).HasColumnName("name");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
                entity.Property(e => e.PasswordSalt).HasColumnName("password_salt");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
