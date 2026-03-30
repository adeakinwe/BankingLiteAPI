namespace BankingLite.Api.Models.DTOs.Request
{
    public class TransactionCreate
    {
        public int Id { get; set; }
        public int SenderUserId { get; set; }
        public int RecipientUserId { get; set; }
        public int SenderAccountId { get; set; }
        public int RecipientAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}