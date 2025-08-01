namespace ShopOnlineMigration.Models;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    
    public virtual ICollection<News> News { get; set; } 
    public virtual ICollection<Product> Products { get; set; }
}