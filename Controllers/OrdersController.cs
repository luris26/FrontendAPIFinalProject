using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerPupusas.Models;
using ServerPupusas.ModelDTOs;
using Newtonsoft.Json;

namespace ServerPupusas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders([FromQuery] string? status = null)
        {
            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Menu)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                orders = orders.Where(o => o.Status == status);

            var result = await orders.Select(order => new OrderDTO
            {
                OrderId = order.OrderId,
                TableId = order.TableId,
                UserId = order.UserId,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    MenuId = oi.MenuId,
                    MenuName = oi.Menu != null ? oi.Menu.Name : "Unknown",
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            }).ToListAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound();

            var orderDto = new OrderDTO
            {
                OrderId = order.OrderId,
                TableId = order.TableId,
                UserId = order.UserId,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt,
                OrderItems = order.OrderItems.Select(item => new OrderItemDTO
                {
                    MenuId = item.MenuId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            return Ok(orderDto);
        }

        [HttpPost("addmenu")]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderDTO newOrderDto)
        {
            try
            {
                Console.WriteLine($"Received Order: {JsonConvert.SerializeObject(newOrderDto)}");

                if (newOrderDto.OrderItems == null || !newOrderDto.OrderItems.Any())
                    return BadRequest("El pedido debe contener al menos un artículo.");

                // Verificar que table_id exista en la tabla referenciada
                var tableExists = await _context.Tables.AnyAsync(t => t.TableId == newOrderDto.TableId);
                if (!tableExists)
                    return BadRequest($"La mesa con ID {newOrderDto.TableId} no existe.");

                var newOrder = new Order
                {
                    TableId = newOrderDto.TableId,
                    UserId = newOrderDto.UserId,
                    Status = newOrderDto.Status ?? "pendiente",
                    TotalAmount = newOrderDto.TotalAmount,
                    CreatedAt = DateTime.UtcNow.ToLocalTime(), // Conversión de fecha
                    OrderItems = newOrderDto.OrderItems.Select(item => new OrderItem
                    {
                        MenuId = item.MenuId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    }).ToList()
                };

                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetOrder), new { id = newOrder.OrderId }, newOrder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating order: {ex.Message}");
                return StatusCode(500, "Internal server error. Please try again.");
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderDTO updatedOrderDto)
        {
            if (id != updatedOrderDto.OrderId)
                return BadRequest("El ID del pedido no coincide.");

            var existingOrder = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (existingOrder == null)
                return NotFound();

            existingOrder.Status = updatedOrderDto.Status ?? existingOrder.Status;
            existingOrder.TotalAmount = updatedOrderDto.TotalAmount ?? existingOrder.TotalAmount;

            existingOrder.OrderItems.Clear();
            foreach (var item in updatedOrderDto.OrderItems)
            {
                existingOrder.OrderItems.Add(new OrderItem
                {
                    MenuId = item.MenuId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("complete/{id}")]
        public async Task<IActionResult> MarkOrderAsCompleted(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            order.Status = "Completed";
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.OrderId == id);
                if (order == null)
                {
                    return NotFound("La orden no existe.");
                }

                // Eliminar los elementos relacionados
                _context.OrderItems.RemoveRange(order.OrderItems);

                // Eliminar la orden
                _context.Orders.Remove(order);

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error eliminando la orden: {ex.Message}");
                return StatusCode(500, "Error interno del servidor. No se pudo eliminar la orden.");
            }
        }

    }
}
