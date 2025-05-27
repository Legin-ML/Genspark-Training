using Microsoft.EntityFrameworkCore;
using NotTwitter.Models;

namespace NotTwitter.Contexts;

public class NotTwitterContext : DbContext
{
    public NotTwitterContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Tweet> Tweets { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Hashtag> Hashtags { get; set; }
    public DbSet<MapHashtags> MapHashtags { get; set; }
    public DbSet<Follow> UserFollowers { get; set; }
}