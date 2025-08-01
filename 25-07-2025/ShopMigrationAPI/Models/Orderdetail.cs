using System;
using System.Collections.Generic;

namespace ShopMigrationAPI.Models;

public partial class Orderdetail
{
    public int Orderid { get; set; }

    public int Productid { get; set; }

    public double? Price { get; set; }

    public int? Quantity { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
