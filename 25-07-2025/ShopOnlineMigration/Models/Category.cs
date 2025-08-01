namespace ShopOnlineMigration.Models;

public sealed class Category
{
    public Category(){
        this.Products = new HashSet<Product>();
    }
    
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public ICollection<Product> Products { get; set; }
}