using System;
using System.Collections.Generic;

namespace CafeteriaAromas.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? SecondLastName { get; set; }

    public string DocumentId { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
