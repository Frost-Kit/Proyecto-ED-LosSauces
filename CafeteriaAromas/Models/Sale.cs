using System;
using System.Collections.Generic;

namespace CafeteriaAromas.Models;

public partial class Sale
{
    public int Id { get; set; }

    public decimal TotalAmount { get; set; }

    public DateOnly SaleDate { get; set; }

    public TimeOnly SaleTime { get; set; }

    public bool IsInvoiced { get; set; }

    public string? InvoiceNumber { get; set; }

    public int? CustomerId { get; set; }

    public int EmployeeId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}
