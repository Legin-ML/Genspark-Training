using AccuNotify.Contexts;
using AccuNotify.Models;

namespace AccuNotify.Repositories;

public class FileModelRepository : Repository<int, FileModel>
{
    public FileModelRepository(AccuNotifyContext context) : base(context)
    {
        
    }
}