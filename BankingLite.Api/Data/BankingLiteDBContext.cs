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
        public DbSet<Transaction> Transaction { get; set; } = null!;

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

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transactions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.SenderId).HasColumnName("sender_id").IsRequired();
                entity.Property(e => e.RecipientId).HasColumnName("recipient_id").IsRequired();
                entity.Property(e => e.SenderAccountId).HasColumnName("sender_account_id").IsRequired();
                entity.Property(e => e.RecipientAccountId).HasColumnName("recipient_account_id").IsRequired();
                entity.Property(e => e.Amount).HasColumnName("amount").HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Timestamp).HasColumnName("timestamp").IsRequired();
                entity.HasOne(e => e.Sender)
                    .WithMany()
                    .HasForeignKey(e => e.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Recipient)
                    .WithMany()
                    .HasForeignKey(e => e.RecipientId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.SenderAccount)
                    .WithMany()
                    .HasForeignKey(e => e.SenderAccountId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.RecipientAccount)
                    .WithMany()
                    .HasForeignKey(e => e.RecipientAccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
