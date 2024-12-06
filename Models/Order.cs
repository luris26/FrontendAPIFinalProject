using System;
using System.Collections.Generic;

namespace ServerPupusas.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? TableId { get; set; }

    public int? UserId { get; set; }

    public string? Status { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Table? Table { get; set; }

    public virtual User? User { get; set; }
    public void SetCreatedAt(DateTime dateTime)
    {
        CreatedAt = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
    }
}
