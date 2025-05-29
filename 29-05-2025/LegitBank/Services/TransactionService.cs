using LegitBank.Interfaces;
using LegitBank.Models;

namespace LegitBank.Services;

public class TransactionService : ITransactionService
{
    private readonly IRepository<int, Transaction> _transactionRepo;
    private readonly IRepository<int, Account> _accountRepo;

    public TransactionService(IRepository<int, Transaction> transactionRepo, IRepository<int, Account> accountRepo)
    {
        _transactionRepo = transactionRepo;
        _accountRepo = accountRepo;
    }

    public async Task<Transaction> Deposit(int accountId, decimal amount)
    {
        var account = await _accountRepo.Get(accountId);
        if (account == null) throw new Exception("Account not found.");

        account.Balance += amount;
        account.ModifiedAt = DateTime.UtcNow;

        await _accountRepo.Update(accountId, account);

        var transaction = new Transaction
        {
            ReceiverAccountId = accountId,
            TransactionType = "Deposit",
            Amount = amount,
            TransactionDate = DateTime.UtcNow
        };

        return await _transactionRepo.Add(transaction);
    }

    public async Task<Transaction> Withdraw(int accountId, decimal amount)
    {
        var account = await _accountRepo.Get(accountId);
        if (account == null) throw new Exception("Account not found.");

        if (account.Balance < amount)
            throw new Exception("Insufficient funds.");

        account.Balance -= amount;
        account.ModifiedAt = DateTime.UtcNow;

        await _accountRepo.Update(accountId, account);

        var transaction = new Transaction
        {
            SenderAccountId = accountId,
            TransactionType = "Withdraw",
            Amount = amount,
            TransactionDate = DateTime.UtcNow
        };

        return await _transactionRepo.Add(transaction);
    }

    public async Task<Transaction> Send(int senderId, int receiverId, decimal amount)
    {
        if (senderId == receiverId)
            throw new Exception("Sender and receiver cannot be the same.");

        var sender = await _accountRepo.Get(senderId);
        var receiver = await _accountRepo.Get(receiverId);

        if (sender == null || receiver == null)
            throw new Exception("One or both accounts not found.");

        if (sender.Balance < amount)
            throw new Exception("Insufficient funds.");

        sender.Balance -= amount;
        sender.ModifiedAt = DateTime.UtcNow;

        receiver.Balance += amount;
        receiver.ModifiedAt = DateTime.UtcNow;

        await _accountRepo.Update(senderId, sender);
        await _accountRepo.Update(receiverId, receiver);

        var transaction = new Transaction
        {
            SenderAccountId = senderId,
            ReceiverAccountId = receiverId,
            TransactionType = "Transfer",
            Amount = amount,
            TransactionDate = DateTime.UtcNow
        };

        return await _transactionRepo.Add(transaction);
    }
    
}
