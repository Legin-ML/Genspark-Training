using System;
using System.Collections.Generic;

namespace ShopMigrationAPI.Models;

public partial class Contactu
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Content { get; set; }
}
