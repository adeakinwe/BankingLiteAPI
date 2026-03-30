namespace BankingLite.Api.Models.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public int SenderAccountId { get; set; }
        public int RecipientAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public User? Sender { get; set; }
        public User? Recipient { get; set; }
        public Account? SenderAccount { get; set; }
        public Account? RecipientAccount { get; set; }
    }
}
