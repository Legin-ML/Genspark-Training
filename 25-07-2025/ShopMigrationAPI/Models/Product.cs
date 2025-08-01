using System;
using System.Collections.Generic;

namespace ShopMigrationAPI.Models;

public partial class Product
{
    public int Productid { get; set; }

    public string? Productname { get; set; }

    public string? Image { get; set; }

    public double? Price { get; set; }

    public int? Userid { get; set; }

    public int? Categoryid { get; set; }

    public int? Colorid { get; set; }

    public int? Modelid { get; set; }

    public int? Storageid { get; set; }

    public DateTime? Sellstartdate { get; set; }

    public DateTime? Sellenddate { get; set; }

    public int? Isnew { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Color? Color { get; set; }

    public virtual Model? Model { get; set; }

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();

    public virtual Storage? Storage { get; set; }

    public virtual User? User { get; set; }
}
