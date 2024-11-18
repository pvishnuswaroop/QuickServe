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

        public async Task<OrderItem> GetOrderItemByIdAsync(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                throw new KeyNotFoundException($"OrderItem with ID {id} not found.");
            }
            return orderItem;
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
    }
}
