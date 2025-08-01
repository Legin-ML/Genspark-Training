using System;
using System.Collections.Generic;

namespace ShopMigrationAPI.Models;

public partial class Color
{
    public int Colorid { get; set; }

    public string? Color1 { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
