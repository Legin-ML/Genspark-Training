namespace LegitBank.Models;

public class Transaction
{
    public int TransactionId { get; set; }
    public int? SenderAccountId { get; set; }
    public int? ReceiverAccountId { get; set; }
    public string TransactionType { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    
    public Account SenderAccount { get; set; }
    public Account ReceiverAccount { get; set; }
}