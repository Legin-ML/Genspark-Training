using Microsoft.EntityFrameworkCore;

public class TrueFeedbackContext : DbContext
{
    public TrueFeedbackContext(DbContextOptions<TrueFeedbackContext> options) : base(options)
    {
        
    }
    
    public DbSet<user> Users { get; set; }
    
}