using System.Collections.Generic;
using System.Threading.Tasks;
using QuickServe.Models;

namespace QuickServe.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<OrderItem?> GetOrderItemByIdAsync(int id);  // Nullable return type for missing order item
        Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);  // Get all items for a specific order
        Task<IEnumerable<OrderItem>> GetOrderItemsByMenuIdAsync(int menuId);  // Get all items for a specific menu item
        Task<IEnumerable<OrderItem>> GetOrderItemsByUserIdAsync(int userId);  // Get all items for a specific user
        Task<OrderItem> AddOrderItemAsync(OrderItem orderItem);
        Task<OrderItem> UpdateOrderItemAsync(OrderItem orderItem);
        Task<bool> DeleteOrderItemAsync(int id);

        // Optional: Pagination support for large result sets
        Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdWithPaginationAsync(int orderId, int page, int pageSize);  // Paginated results for order items by order
    }
}
