using LegitBank.Interfaces;
using LegitBank.Models;
using LegitBank.Models.Dtos;

namespace LegitBank.Services;

public class AccountService : IAccountService
{
    private readonly IRepository<int, Account> _repository;

    public AccountService(IRepository<int, Account> repository)
    {
        _repository = repository;
    }

    public async Task<Account> CreateAccount(AccountCreateDto dto)
    {
        var account = new Account
        {
            AccountName = dto.AccountName,
            AccountType = dto.AccountType,
            Balance = 0m,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

        return await _repository.Add(account);
    }

    public async Task<Account> GetAccount(int accountId)
    {
        return await _repository.Get(accountId);
    }
    

    public async Task<Account> DeleteAccount(int accountId)
    {
        return await _repository.Delete(accountId);
    }
}