using System;
using System.Collections.Generic;

namespace ShopMigrationAPI.Models;

public partial class User
{
    public int Userid { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
