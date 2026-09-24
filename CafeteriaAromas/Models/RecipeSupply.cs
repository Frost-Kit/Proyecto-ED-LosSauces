using System;
using System.Collections.Generic;

namespace CafeteriaAromas.Models;

public partial class RecipeSupply
{
    public int Id { get; set; }

    public decimal IngredientQuantity { get; set; }

    public int SupplyId { get; set; }

    public int RecipeId { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;

    public virtual Supply Supply { get; set; } = null!;
}
