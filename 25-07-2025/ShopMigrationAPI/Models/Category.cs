using System;
using System.Collections.Generic;

namespace ShopMigrationAPI.Models;

public partial class Category
{
    public int Categoryid { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
