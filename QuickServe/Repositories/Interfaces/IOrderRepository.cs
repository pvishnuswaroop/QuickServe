using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuickServe.Models;

namespace QuickServe.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> AddOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId); // Additional method
        Task<IEnumerable<Order>> GetOrdersByRestaurantIdAsync(int restaurantId); // Orders by restaurant
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status); // Orders by status
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate); // Orders within a date range
    }
}
