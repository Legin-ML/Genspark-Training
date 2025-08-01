using System;
using System.Collections.Generic;

namespace ShopMigrationAPI.Models;

public partial class Storage
{
    public int Storageid { get; set; }

    public string? Storage1 { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
