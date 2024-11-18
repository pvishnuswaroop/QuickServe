using System.Collections.Generic;
using System.Threading.Tasks;
using QuickServe.Models;

namespace QuickServe.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<OrderItem> GetOrderItemByIdAsync(int id);
        Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);  // Get all items in a specific order
        Task<IEnumerable<OrderItem>> GetOrderItemsByMenuIdAsync(int menuId);      // Get all items for a specific menu item
        Task<IEnumerable<OrderItem>> GetOrderItemsByUserIdAsync(int userId);      // Get all items for a specific user
        Task<OrderItem> AddOrderItemAsync(OrderItem orderItem);
        Task<OrderItem> UpdateOrderItemAsync(OrderItem orderItem);
        Task<bool> DeleteOrderItemAsync(int id);
    }
}
