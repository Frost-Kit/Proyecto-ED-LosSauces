using System;
using System.Collections.Generic;

namespace CafeteriaAromas.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? SecondLastName { get; set; }

    public string DocumentId { get; set; } = null!;

    public string? Phone { get; set; }

    public decimal Salary { get; set; }

    public string? Email { get; set; }

    public int JobPositionId { get; set; }

    public virtual JobPosition JobPosition { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
