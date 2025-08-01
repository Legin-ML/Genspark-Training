using ShopMigrationAPI.Models;

namespace ShopMigrationAPI.Repositories;

public class ContactUsRepository(ShopMigrationDbContext context) : Repository<Contactu>(context);