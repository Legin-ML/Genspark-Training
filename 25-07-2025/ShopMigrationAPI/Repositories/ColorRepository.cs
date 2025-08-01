using ShopMigrationAPI.Models;

namespace ShopMigrationAPI.Repositories;

public class ColorRepository(ShopMigrationDbContext context) : Repository<Color>(context);