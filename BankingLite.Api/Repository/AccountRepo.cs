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

        public async Task<AccountRead> CreateAccountAsync(AccountCreate entity)
        {
            var userAccountExists = await _context.Account
                .AnyAsync(a => a.UserId == entity.UserId && a.AccountNumber == entity.AccountNumber);
            if (userAccountExists)                
            throw new InvalidOperationException("An account with the same account number already exists for this user.");   
            
            var account = new Account
            {
                UserId = entity.UserId,
                BankName = entity.BankName,
                AccountNumber = entity.AccountNumber,
                Balance = entity.Balance
            };

            _context.Account.Add(account);
            await _context.SaveChangesAsync();

            var createdAccount = new AccountRead
            {
                AccountId = account.AccountId,
                UserId = account.UserId,
                BankName = account.BankName,
                AccountNumber = account.AccountNumber,
                Balance = account.Balance
            };

            return createdAccount;
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

        public async Task<AccountRead> UpdateAccountAsync(AccountCreate account, int accountId)
        {
            var existingAccount = await _context.Account.FindAsync(accountId);
            if (existingAccount == null) throw new KeyNotFoundException($"Account with ID {accountId} not found.");

            existingAccount.BankName = account.BankName;
            existingAccount.AccountNumber = account.AccountNumber;
            existingAccount.Balance = account.Balance;

            await _context.SaveChangesAsync();
            return new AccountRead
            {
                AccountId = existingAccount.AccountId,
                UserId = existingAccount.UserId,
                BankName = existingAccount.BankName,
                AccountNumber = existingAccount.AccountNumber,
                Balance = existingAccount.Balance
            };
        }
    }
}