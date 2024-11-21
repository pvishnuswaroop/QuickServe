using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuickServe.Data;
using QuickServe.Models;
using QuickServe.Repositories.Interfaces;

namespace QuickServe.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)  // Eager load related OrderItems
                .Include(o => o.User)  // Eager load User
                .Include(o => o.Restaurant)  // Eager load Restaurant
                .Include(o => o.Payment)  // Eager load Payment
                .FirstOrDefaultAsync(o => o.OrderID == id);

            return order;  // Return null if not found, no need for exception handling
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)  // Eager load OrderItems
                .Include(o => o.User)  // Eager load User
                .Include(o => o.Restaurant)  // Eager load Restaurant
                .Include(o => o.Payment)  // Eager load Payment
                .ToListAsync();
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserID == userId)
                .Include(o => o.OrderItems)  // Eager load related OrderItems
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Orders
                .Where(o => o.RestaurantID == restaurantId)
                .Include(o => o.OrderItems)  // Eager load related OrderItems
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
        {
            if (Enum.TryParse(status, ignoreCase: true, out OrderStatus parsedStatus))
            {
                return await _context.Orders
                    .Where(o => o.OrderStatus == parsedStatus)
                    .Include(o => o.OrderItems)  // Eager load related OrderItems
                    .ToListAsync();
            }
            return Enumerable.Empty<Order>();  // Return an empty list if status is invalid
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be later than end date.");
            }

            return await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .Include(o => o.OrderItems)  // Eager load related OrderItems
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersWithPaginationAsync(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("Page and PageSize must be greater than zero.");
            }

            return await _context.Orders
                .Skip((page - 1) * pageSize)  // Skip to the correct page
                .Take(pageSize)  // Limit the number of results per page
                .Include(o => o.OrderItems)  // Eager load related OrderItems
                .ToListAsync();
        }
    }
}
