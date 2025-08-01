using System;
using System.Collections.Generic;

namespace ShopMigrationAPI.Models;

public partial class News
{
    public int Newsid { get; set; }

    public int? Userid { get; set; }

    public string? Title { get; set; }

    public string? Shortdescription { get; set; }

    public string? Image { get; set; }

    public string? Content { get; set; }

    public DateTime? Createddate { get; set; }

    public int? Status { get; set; }

    public virtual User? User { get; set; }
}
