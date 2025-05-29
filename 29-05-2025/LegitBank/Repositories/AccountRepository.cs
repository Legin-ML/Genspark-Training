using LegitBank.Contexts;
using LegitBank.Models;

namespace LegitBank.Repositories;

public class AccountRepository : Repository<int, Account>
{
    public AccountRepository(LegitBankContext context) : base(context)
    {
        
    }
}