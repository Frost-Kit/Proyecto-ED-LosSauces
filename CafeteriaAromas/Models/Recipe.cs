using System;
using System.Collections.Generic;

namespace CafeteriaAromas.Models;

public partial class Recipe
{
    public int Id { get; set; }

    public string Instructions { get; set; } = null!;

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<RecipeSupply> RecipeSupplies { get; set; } = new List<RecipeSupply>();
}
