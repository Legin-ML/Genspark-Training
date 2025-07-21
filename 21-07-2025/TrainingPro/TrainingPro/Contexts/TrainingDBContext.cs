using Microsoft.EntityFrameworkCore;
using TrainingPro.Models;

namespace TrainingPro.Contexts;

public class TrainingDBContext : DbContext
{
    public TrainingDBContext(DbContextOptions<TrainingDBContext> options) : base(options)
    {
        
    }
    
    public DbSet<TrainingVideo>  TrainingVideos { get; set; }
}