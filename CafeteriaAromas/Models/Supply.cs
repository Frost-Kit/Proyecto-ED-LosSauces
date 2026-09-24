using System;
using System.Collections.Generic;

namespace CafeteriaAromas.Models;

public partial class Supply
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string UnitOfMeasure { get; set; } = null!;

    public int SupplyCategoryId { get; set; }

    public decimal StoredQuantity { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<RecipeSupply> RecipeSupplies { get; set; } = new List<RecipeSupply>();

    public virtual SupplyCategory SupplyCategory { get; set; } = null!;
}
