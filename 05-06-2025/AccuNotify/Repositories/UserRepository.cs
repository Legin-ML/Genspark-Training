using AccuNotify.Contexts;
using AccuNotify.Models;

namespace AccuNotify.Repositories;

public class UserRepository : Repository<int, User>
{
    public UserRepository(AccuNotifyContext context) : base(context)
    {
        
    }
}