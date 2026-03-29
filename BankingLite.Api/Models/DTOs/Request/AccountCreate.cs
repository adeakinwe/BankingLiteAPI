namespace BankingLite.Api.Models.DTOs.Request
{
    public class AccountCreate
    {
        public int UserId { get; set; }
        public string BankName { get; set; } = null!;
        public string AccountNumber { get; set; } = null!;
        public decimal Balance { get; set; }
    }
}