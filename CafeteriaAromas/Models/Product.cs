using System;
using System.Collections.Generic;

namespace CafeteriaAromas.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal SellingPrice { get; set; }

    public decimal ProductionCost { get; set; }

    public int ProductCategoryId { get; set; }

    public virtual ProductCategory ProductCategory { get; set; } = null!;

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}
