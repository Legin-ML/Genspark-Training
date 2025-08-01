using ShopMigrationAPI.Models;

namespace ShopMigrationAPI.Repositories;

public class CategoryRepository(ShopMigrationDbContext context) : Repository<Category>(context)
{
}