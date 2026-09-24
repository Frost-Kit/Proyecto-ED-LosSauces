using System;
using System.Collections.Generic;

namespace CafeteriaAromas.Models;

public partial class InventoryPurchase
{
    public int Id { get; set; }

    public decimal Quantity { get; set; }

    public string UnitOfMeasure { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateOnly PurchaseDate { get; set; }

    public int SupplierId { get; set; }

    public int InventoryId { get; set; }

    public virtual Inventory Inventory { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
