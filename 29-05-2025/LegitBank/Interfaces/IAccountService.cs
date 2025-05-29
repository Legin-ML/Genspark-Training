using LegitBank.Models;
using LegitBank.Models.Dtos;

namespace LegitBank.Interfaces;

public interface IAccountService
{
    Task<Account> CreateAccount(AccountCreateDto dto);
    Task<Account> GetAccount(int accountId);
    Task<Account> DeleteAccount(int accountId);
}