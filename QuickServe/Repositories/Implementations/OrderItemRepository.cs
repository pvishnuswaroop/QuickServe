using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuickServe.Data;
using QuickServe.Models;
using QuickServe.Repositories.Interfaces;

namespace QuickServe.Repositories.Implementations
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _context;

        public OrderItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrderItem?> GetOrderItemByIdAsync(int id)
        {
            return await _context.OrderItems.FindAsync(id);  // Return null if not found instead of throwing an exception
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                .Where(oi => oi.OrderID == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByMenuIdAsync(int menuId)
        {
            return await _context.OrderItems
                .Where(oi => oi.MenuID == menuId)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByUserIdAsync(int userId)
        {
            return await _context.OrderItems
                .Where(oi => oi.Order != null && oi.Order.UserID == userId)
                .ToListAsync();
        }

        public async Task<OrderItem> AddOrderItemAsync(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }

        public async Task<OrderItem> UpdateOrderItemAsync(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }

        public async Task<bool> DeleteOrderItemAsync(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null) return false;

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdWithPaginationAsync(int orderId, int page, int pageSize)
        {
            return await _context.OrderItems
                .Where(oi => oi.OrderID == orderId)
                .Skip((page - 1) * pageSize)  // Skip to the correct page
                .Take(pageSize)  // Limit the number of results per page
                .ToListAsync();
        }
    }
}