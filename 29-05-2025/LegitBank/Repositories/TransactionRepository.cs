using LegitBank.Contexts;
using LegitBank.Interfaces;
using LegitBank.Models;

namespace LegitBank.Repositories;

public class TransactionRepository : Repository<int, Transaction>
{
    public TransactionRepository(LegitBankContext context) : base(context)
    {
        
    }
}