using QuickServe.Data;
using QuickServe.Models;
using QuickServe.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickServe.Repositories.Implementations
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly AppDbContext _context;

        public CartItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem> GetCartItemByIdAsync(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                throw new KeyNotFoundException($"CartItem with ID {id} not found.");
            }
            return cartItem;
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId)
        {
            return await _context.CartItems
                .Where(c => c.CartID == cartId)
                .ToListAsync();
        }

        public async Task<CartItem> AddCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<CartItem> UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<bool> DeleteCartItemAsync(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null) return false;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CartItem> UpdateCartItemQuantityAsync(int cartItemId, int newQuantity)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null)
            {
                throw new KeyNotFoundException($"CartItem with ID {cartItemId} not found.");
            }

            if (newQuantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            cartItem.Quantity = newQuantity;
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<CartItem> UpdateCartItemPriceAsync(int cartItemId, decimal newPrice)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null)
            {
                throw new KeyNotFoundException($"CartItem with ID {cartItemId} not found.");
            }

            if (newPrice <= 0)
            {
                throw new ArgumentException("Price must be greater than zero.");
            }

            cartItem.Menu.Price = newPrice;  // Assuming the price is part of the Menu entity
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<bool> ClearCartAsync(int cartId)
        {
            var cartItems = await _context.CartItems.Where(c => c.CartID == cartId).ToListAsync();
            if (!cartItems.Any())
            {
                return false;  // No items to clear
            }

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return true;
        }

        // Implementing the GetCartItemsPaginatedAsync method
        public async Task<IEnumerable<CartItem>> GetCartItemsPaginatedAsync(int cartId, int pageNumber, int pageSize)
        {
            // Validate the page number and page size
            if (pageNumber < 1) pageNumber = 1; // Ensure the page number is at least 1
            if (pageSize < 1) pageSize = 10; // Default page size

            // Calculate the items to skip based on the page number and page size
            var skip = (pageNumber - 1) * pageSize;

            // Query to fetch the paginated results
            var paginatedItems = await _context.CartItems
                .Where(c => c.CartID == cartId)
                .Skip(skip) // Skip the items based on the current page
                .Take(pageSize) // Take the number of items based on page size
                .ToListAsync();

            return paginatedItems;
        }
    }
}
