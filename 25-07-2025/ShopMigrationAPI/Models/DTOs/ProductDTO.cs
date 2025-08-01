namespace ShopMigrationAPI.Models.DTOs;

public class ProductDTO
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
}