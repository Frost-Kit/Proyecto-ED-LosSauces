using System;
using System.Collections.Generic;

namespace CafeteriaAromas.Models;

public partial class SupplyCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Supply> Supplies { get; set; } = new List<Supply>();
}
