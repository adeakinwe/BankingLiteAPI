namespace BankingLite.Api.Models.DTOs.Response
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}