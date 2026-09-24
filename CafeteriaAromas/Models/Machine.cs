using System;
using System.Collections.Generic;

namespace CafeteriaAromas.Models;

public partial class Machine
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string MachineFunction { get; set; } = null!;

    public string SerialNumber { get; set; } = null!;

    public int WarehouseId { get; set; }

    public virtual ICollection<MachineMaintenance> MachineMaintenances { get; set; } = new List<MachineMaintenance>();

    public virtual Warehouse Warehouse { get; set; } = null!;
}
