namespace BankingLite.Api.Models.DTOs.Response
{
    public class AccountRead
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public string BankName { get; set; } = null!;
        public string AccountNumber { get; set; } = null!;
        public decimal Balance { get; set; }
    }
}