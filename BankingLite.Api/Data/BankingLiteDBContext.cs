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
        public DbSet<Account> Account { get; set; } = null!;

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

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account");
                entity.HasKey(e => e.AccountId);
                entity.Property(e => e.AccountId).HasColumnName("account_id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.BankName).HasColumnName("bank_name").IsRequired();
                entity.Property(e => e.AccountNumber).HasColumnName("account_number").IsRequired();
                entity.Property(e => e.Balance).HasColumnName("balance").HasColumnType("decimal(18,2)").IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
