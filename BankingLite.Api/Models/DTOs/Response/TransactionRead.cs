namespace BankingLite.Api.Models.DTOs.Response
{
    public class TransactionRead
    {
        public string SenderName { get; set; } = null!;
        public string RecipientName { get; set; } = null!;
        public string SenderAccountNumber { get; set; } = null!;
        public string RecipientAccountNumber { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }

    }
}