using BankingLite.Api.Data;
using BankingLite.Api.Interface;
using BankingLite.Api.Models.DTOs.Request;
using BankingLite.Api.Models.DTOs.Response;
using BankingLite.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingLite.Api.Repository
{
    public class AccountRepo : IAccountRepo
    {
        private readonly BankingLiteDbContext _context;

        public AccountRepo(BankingLiteDbContext context)
        {
            _context = context;
        }

        public async Task<AccountCreate> CreateAccountAsync(AccountCreate entity)
        {
            var account = new Account
            {
                UserId = entity.UserId,
                BankName = entity.BankName,
                AccountNumber = entity.AccountNumber,
                Balance = entity.Balance
            };

            _context.Account.Add(account);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<AccountRead?> GetAccountByIdAsync(int accountId)
        {
            var account = await _context.Account.FindAsync(accountId);
            if (account == null) return null;

            return new AccountRead
            {
                AccountId = account.AccountId,
                UserId = account.UserId,
                BankName = account.BankName,
                AccountNumber = account.AccountNumber,
                Balance = account.Balance
            };
        }

        public async Task<IEnumerable<AccountRead?>> GetAccountsByUserIdAsync(int userId)
        {
            var accounts = await _context.Account.Where(a => a.UserId == userId).ToListAsync();
            if (accounts == null || accounts.Count == 0) return Enumerable.Empty<AccountRead>();
            return accounts.Select(account => new AccountRead
            {
                AccountId = account.AccountId,
                UserId = account.UserId,
                BankName = account.BankName,
                AccountNumber = account.AccountNumber,
                Balance = account.Balance
            });
        }

        public async Task<AccountCreate> UpdateAccountAsync(AccountCreate account, int accountId)
        {
            var existingAccount = await _context.Account.FindAsync(accountId);
            if (existingAccount == null) throw new KeyNotFoundException($"Account with ID {accountId} not found.");

            existingAccount.BankName = account.BankName;
            existingAccount.AccountNumber = account.AccountNumber;
            existingAccount.Balance = account.Balance;

            await _context.SaveChangesAsync();
            return account;
        }
    }
}