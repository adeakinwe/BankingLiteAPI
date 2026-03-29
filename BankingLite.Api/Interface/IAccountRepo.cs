using BankingLite.Api.Models.DTOs.Request;
using BankingLite.Api.Models.DTOs.Response;

namespace BankingLite.Api.Interface
{
    public interface IAccountRepo
    {
        Task<AccountCreate> CreateAccountAsync(AccountCreate entity);
        Task<AccountRead?> GetAccountByIdAsync(int accountId);
        Task<IEnumerable<AccountRead?>> GetAccountsByUserIdAsync(int userId);
        Task<AccountCreate> UpdateAccountAsync(AccountCreate account, int accountId);
    }
}