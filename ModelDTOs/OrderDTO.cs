using System;
using System.Collections.Generic;

namespace ServerPupusas.ModelDTOs
{
    public class OrderDTO
    {
        public int? OrderId { get; set; }
        public int? TableId { get; set; }
        public int? UserId { get; set; }
        public string? Status { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
    }
}
