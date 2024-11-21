using ServerPupusas.ModelDTOs;
using ServerPupusas.Models;

namespace FrontendAPIFinalProject.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrders(string? status = null);
        Task<Order?> GetOrderById(int id);
        Task<Order> CreateOrder(OrderDTO newOrder);
        Task<bool> UpdateOrder(int id, OrderDTO updatedOrder);
        Task<bool> MarkOrderAsCompleted(int id);
        Task<bool> DeleteOrder(int id);
    }
}
