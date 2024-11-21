using FrontendAPIFinalProject.Services;
using Microsoft.EntityFrameworkCore;
using ServerPupusas.ModelDTOs;
using ServerPupusas.Models;

namespace ServerPupusas.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrders(string? status = null)
        {
            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Menu)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                orders = orders.Where(o => o.Status == status);

            return await orders.ToListAsync();
        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Menu)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<Order> CreateOrder(OrderDTO newOrderDto)
        {
            var newOrder = new Order
            {
                TableId = newOrderDto.TableId,
                UserId = newOrderDto.UserId,
                Status = newOrderDto.Status ?? "Pending",
                TotalAmount = newOrderDto.TotalAmount ?? newOrderDto.OrderItems.Sum(oi => oi.Price * oi.Quantity),
                CreatedAt = DateTime.UtcNow,
                OrderItems = newOrderDto.OrderItems.Select(oi => new OrderItem
                {
                    MenuId = oi.MenuId,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };

            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            return newOrder;
        }

        public async Task<bool> UpdateOrder(int id, OrderDTO updatedOrderDto)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (existingOrder == null)
                return false;

            existingOrder.Status = updatedOrderDto.Status ?? existingOrder.Status;
            existingOrder.TotalAmount = updatedOrderDto.TotalAmount ?? existingOrder.TotalAmount;

            existingOrder.OrderItems.Clear();
            foreach (var orderItem in updatedOrderDto.OrderItems)
            {
                existingOrder.OrderItems.Add(new OrderItem
                {
                    MenuId = orderItem.MenuId,
                    Quantity = orderItem.Quantity,
                    Price = orderItem.Price
                });
            }


            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkOrderAsCompleted(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return false;

            order.Status = "Completed";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
