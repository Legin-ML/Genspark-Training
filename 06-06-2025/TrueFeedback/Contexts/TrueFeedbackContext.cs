using Microsoft.EntityFrameworkCore;
using TrueFeedback.Models;

namespace TrueFeedback.Contexts;

public class TrueFeedbackContext : DbContext
{
    public TrueFeedbackContext(DbContextOptions<TrueFeedbackContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<Role> Roles { get; set; }
    
}