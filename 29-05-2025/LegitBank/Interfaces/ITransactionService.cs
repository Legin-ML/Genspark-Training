using LegitBank.Models;

namespace LegitBank.Interfaces;

public interface ITransactionService
{
    Task<Transaction> Deposit(int accountId, decimal amount);
    Task<Transaction> Withdraw(int accountId, decimal amount);
    Task<Transaction> Send(int senderId, int receiverId, decimal amount);
    // Task<IEnumerable<Transaction>> GetTransactionsForAccount(int accountId);
}