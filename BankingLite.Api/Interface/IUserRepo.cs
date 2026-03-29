using BankingLite.Api.Models.DTOs.Request;
using BankingLite.Api.Models.DTOs.Response;

namespace BankingLite.Api.Interface
{
    public interface IUserRepo
    {
        Task<UserResponse> Register(RegisterRequest request);
        Task<UserResponse> Login(LoginRequest request);
        Task<UserResponse> GetUserById(int id);
    }
}