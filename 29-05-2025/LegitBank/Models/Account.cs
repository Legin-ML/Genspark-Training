namespace LegitBank.Models;

public class Account
{
    public int AccountId { get; set; }
    public string AccountName { get; set; } 
    public decimal Balance { get; set; }
    public string AccountType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    
    public ICollection<Transaction> SentTransactions { get; set; }
    public ICollection<Transaction> ReceivedTransactions { get; set; }
}