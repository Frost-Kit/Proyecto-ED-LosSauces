using System;
using System.Collections.Generic;

namespace CafeteriaAromas.Models;

public partial class JobPosition
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
