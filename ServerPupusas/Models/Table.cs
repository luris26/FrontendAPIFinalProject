using System;
using System.Collections.Generic;

namespace ServerPupusas.Models;

public partial class Table
{
    public int TableId { get; set; }

    public int Number { get; set; }

    public string? Status { get; set; }

    public int Capacity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
