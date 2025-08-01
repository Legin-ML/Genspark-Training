using Microsoft.EntityFrameworkCore;
using ShopMigrationAPI.Interfaces.Repositories;
using ShopMigrationAPI.Models;

namespace ShopMigrationAPI.Repositories;

public class NewsRepository(ShopMigrationDbContext context) : Repository<News>(context)
{
    public IEnumerable<News> GetAllWithUsers()
    {
        return _context.News.Include(n => n.User).ToList();
    }
}