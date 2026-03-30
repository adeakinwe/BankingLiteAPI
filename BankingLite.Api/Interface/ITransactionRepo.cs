using System.Transactions;
using BankingLite.Api.Models.DTOs.Request;
using BankingLite.Api.Models.DTOs.Response;

namespace BankingLite.Api.Interface
{
    public interface ITransactionRepo
    {
        Task<TransactionRead> TransferFundAsync(TransactionCreate request);
        Task<IEnumerable<TransactionRead>> GetTransactionsByUserIdAsync(int userId);
        Task<IEnumerable<TransactionRead>> GetTransactionsByAccountIdAsync(int accountId);
    }
}