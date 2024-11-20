using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuickServe.Models;

namespace QuickServe.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderByIdAsync(int id);  // Use nullable to return null if order not found
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> AddOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);  // Orders by User
        Task<IEnumerable<Order>> GetOrdersByRestaurantIdAsync(int restaurantId);  // Orders by Restaurant
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);  // Orders by Status
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);  // Orders within a Date Range

        // Optional: Add pagination parameters
        Task<IEnumerable<Order>> GetOrdersWithPaginationAsync(int page, int pageSize);  // For paginated results
    }
}
