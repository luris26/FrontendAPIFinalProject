using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ServerPupusas.ModelDTOs
{
    public class OrderDTO
    {
        public int? OrderId { get; set; }
        public int? TableId { get; set; }
        public int? UserId { get; set; }
        public string? Status { get; set; } = "pendiente";
        public decimal? TotalAmount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
        public void SetCreatedAt(DateTime dateTime)
        {
            CreatedAt = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
        }
    }
}
