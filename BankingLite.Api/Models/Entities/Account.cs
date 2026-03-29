namespace BankingLite.Api.Models.Entities
{
    public class Account
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public string BankName { get; set; } = null!;
        public string AccountNumber { get; set; } = null!;
        public decimal Balance { get; set; }
        public User User { get; set; } = null!;
    }
}