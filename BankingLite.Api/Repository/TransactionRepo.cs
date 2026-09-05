using BankingLite.Api.Data;
using BankingLite.Api.Interface;
using BankingLite.Api.Models.DTOs.Request;
using BankingLite.Api.Models.DTOs.Response;
using BankingLite.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingLite.Api.Repository
{
    public class TransactionRepo : ITransactionRepo
    {
        public readonly BankingLiteDbContext _context;
        public TransactionRepo(BankingLiteDbContext context)
        {
            _context = context;
        }

        public async Task<TransactionRead> TransferFundAsync(TransactionCreate request)
        {
            if (request.Amount <= 0)
                throw new ArgumentException("Transfer amount must be positive.", nameof(request));

            var senderAccount = await _context.Account.SingleOrDefaultAsync(a => a.AccountId == request.SenderAccountId);
            if (senderAccount == null)
                throw new InvalidOperationException("Sender account not found.");

            var recipientAccount = await _context.Account.SingleOrDefaultAsync(a => a.AccountId == request.RecipientAccountId);
            if (recipientAccount == null)
                throw new InvalidOperationException("Recipient account not found.");

            var senderUser = await _context.User.SingleOrDefaultAsync(u => u.Id == senderAccount.UserId);
            if (senderUser == null)
                throw new InvalidOperationException("Sender user not found.");

            var recipientUser = await _context.User.SingleOrDefaultAsync(u => u.Id == recipientAccount.UserId);
            if (recipientUser == null)
                throw new InvalidOperationException("Recipient user not found.");

            if (senderAccount.AccountId == recipientAccount.AccountId)
                throw new InvalidOperationException("Cannot transfer to the same account.");

            if (senderAccount.Balance < request.Amount)
                throw new InvalidOperationException("Insufficient balance.");

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                senderAccount.Balance -= request.Amount;
                recipientAccount.Balance += request.Amount;

                _context.Account.Update(senderAccount);
                _context.Account.Update(recipientAccount);

                var transfer = new Transaction
                {
                    SenderId = senderAccount.UserId,
                    RecipientId = recipientAccount.UserId,
                    SenderAccountId = senderAccount.AccountId,
                    RecipientAccountId = recipientAccount.AccountId,
                    Amount = request.Amount,
                    Timestamp = DateTime.UtcNow
                };

                _context.Transaction.Add(transfer);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var transferCompletedRead = new TransactionRead
                {
                    SenderName = senderUser.FullName,
                    RecipientName = recipientUser.FullName,
                    SenderAccountNumber = senderAccount.AccountNumber,
                    RecipientAccountNumber = recipientAccount.AccountNumber,
                    Amount = transfer.Amount,
                    Timestamp = transfer.Timestamp
                };

                return transferCompletedRead;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<TransactionRead>> GetTransactionsByUserIdAsync(int userId)
        {
            return await (from a in _context.Transaction
                         join b in _context.Account on a.SenderAccountId equals b.AccountId
                         join c in _context.Account on a.RecipientAccountId equals c.AccountId
                         where a.SenderId == userId || a.RecipientId == userId
                         orderby a.Timestamp descending
                         select new TransactionRead
                         {
                             SenderName = b.User.FullName,
                             RecipientName = c.User.FullName,
                             SenderAccountNumber = b.AccountNumber,
                             RecipientAccountNumber = c.AccountNumber,
                             Amount = a.Amount,
                             Timestamp = a.Timestamp
                         }).ToListAsync();
        }

        public async Task<IEnumerable<TransactionRead>> GetTransactionsByAccountIdAsync(int accountId)
        {
            return await (from a in _context.Transaction
                         join b in _context.Account on a.SenderAccountId equals b.AccountId
                         join c in _context.Account on a.RecipientAccountId equals c.AccountId
                         where a.SenderAccountId == accountId || a.RecipientAccountId == accountId
                         orderby a.Timestamp descending
                         select new TransactionRead
                         {
                             SenderName = b.User.FullName,
                             RecipientName = c.User.FullName,
                             SenderAccountNumber = b.AccountNumber,
                             RecipientAccountNumber = c.AccountNumber,
                             Amount = a.Amount,
                             Timestamp = a.Timestamp
                         }).ToListAsync();
        }
    }
}