using ShopMigrationAPI.Models;

namespace ShopMigrationAPI.Repositories;

public class ProductRepository(ShopMigrationDbContext context) : Repository<Product>(context);