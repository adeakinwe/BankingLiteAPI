namespace BankingLite.Api.Models.DTOs.Request
{
    public class TransactionCreate
    {
        public int SenderAccountId { get; set; }
        public int RecipientAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}