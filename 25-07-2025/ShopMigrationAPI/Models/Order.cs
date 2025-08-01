using System;
using System.Collections.Generic;

namespace ShopMigrationAPI.Models;

public partial class Order
{
    public int Orderid { get; set; }

    public string? Ordername { get; set; }

    public DateOnly? Orderdate { get; set; }

    public string? Paymenttype { get; set; }

    public string? Status { get; set; }

    public string? Customername { get; set; }

    public string? Customerphone { get; set; }

    public string? Customeremail { get; set; }

    public string? Customeraddress { get; set; }

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();
}
