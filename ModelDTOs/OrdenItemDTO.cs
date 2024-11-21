using System;

namespace ServerPupusas.ModelDTOs
{
    public class OrderItemDTO
    {
        public int? MenuId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string MenuName { get; set; } = string.Empty;
    }
}
