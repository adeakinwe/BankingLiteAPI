using System.Security.Cryptography;
using BankingLite.Api.Data;
using BankingLite.Api.Interface;
using BankingLite.Api.Models.DTOs.Request;
using BankingLite.Api.Models.DTOs.Response;
using BankingLite.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingLite.Api.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly BankingLiteDbContext _dbContext;

        public UserRepo(BankingLiteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserResponse> GetUserById(int id)
        {
            return await _dbContext.Users
                .Where(u => u.Id == id)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email
                })
                .FirstOrDefaultAsync() ?? throw new Exception("User not found");
        }

        public async Task<UserResponse> Login(LoginRequest request)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                throw new Exception("User not found");

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Invalid password");

            var userResponse = new UserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email
            };
            
            return userResponse;
        }

        public async Task<UserResponse> Register(RegisterRequest request)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email))
                throw new InvalidOperationException("Email is already registered.");

            CreatePasswordHash(request.Password, out var hash, out var salt);

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var userResponse = new UserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email
            };
            return userResponse;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }
    }
}