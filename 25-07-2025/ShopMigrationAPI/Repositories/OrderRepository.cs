using ShopMigrationAPI.Models;

namespace ShopMigrationAPI.Repositories;

public class OrderRepository(ShopMigrationDbContext context) : Repository<Order>(context);