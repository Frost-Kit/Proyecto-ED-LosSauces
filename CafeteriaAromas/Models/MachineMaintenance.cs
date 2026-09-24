using System;
using System.Collections.Generic;

namespace CafeteriaAromas.Models;

public partial class MachineMaintenance
{
    public int Id { get; set; }

    public string Diagnosis { get; set; } = null!;

    public DateOnly MaintenanceDate { get; set; }

    public decimal Cost { get; set; }

    public int MachineId { get; set; }

    public virtual Machine Machine { get; set; } = null!;
}
