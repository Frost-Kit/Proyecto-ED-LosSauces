using System;
using System.Collections.Generic;

namespace CafeteriaAromas.Models;

public partial class Supplier
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Nit { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<InventoryPurchase> InventoryPurchases { get; set; } = new List<InventoryPurchase>();
}
