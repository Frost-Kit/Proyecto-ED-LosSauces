using System;
using System.Collections.Generic;

namespace CafeteriaAromas.Models;

public partial class Warehouse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Machine> Machines { get; set; } = new List<Machine>();
}
