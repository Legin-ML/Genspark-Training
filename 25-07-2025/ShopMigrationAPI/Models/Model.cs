using System;
using System.Collections.Generic;

namespace ShopMigrationAPI.Models;

public partial class Model
{
    public int Modelid { get; set; }

    public string? Model1 { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
