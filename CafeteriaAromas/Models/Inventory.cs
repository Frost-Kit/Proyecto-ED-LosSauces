using System;
using System.Collections.Generic;

namespace CafeteriaAromas.Models;

public partial class Inventory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public string UnitOfMeasure { get; set; } = null!;

    public decimal Stock { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public DateOnly? ProductionDate { get; set; }

    public int SupplyId { get; set; }

    public virtual ICollection<InventoryPurchase> InventoryPurchases { get; set; } = new List<InventoryPurchase>();

    public virtual Supply Supply { get; set; } = null!;
}
