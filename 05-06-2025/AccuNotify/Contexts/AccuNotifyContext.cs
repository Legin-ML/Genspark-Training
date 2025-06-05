using AccuNotify.Models;
using Microsoft.EntityFrameworkCore;

namespace AccuNotify.Contexts;

public class AccuNotifyContext : DbContext
{
    public AccuNotifyContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<FileModel> Files { get; set; }
    public DbSet<User> Users { get; set; }
}