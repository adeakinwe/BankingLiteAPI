namespace BankingLite.Api.Models.DTOs.Response
{
    public class Authresponse
    {
        
        public int userId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}