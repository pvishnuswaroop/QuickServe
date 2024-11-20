using Microsoft.EntityFrameworkCore;
using QuickServe.Data;
using QuickServe.Models;
using QuickServe.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickServe.Repositories.Implementations
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserID == userId); // Assuming one cart per user
            if (cart == null)
            {
                throw new KeyNotFoundException($"Cart for user with ID {userId} not found.");
            }
            return cart;
        }

        public async Task<IEnumerable<Cart>> GetAllCartsAsync()
        {
            return await _context.Carts.ToListAsync();
        }

        public async Task<Cart> AddCartAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart> UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<bool> DeleteCartAsync(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null) return false;

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return true;
        }

        // Get carts with pagination
        public async Task<IEnumerable<Cart>> GetCartsPaginatedAsync(int pageNumber, int pageSize)
        {
            // Calculate the number of items to skip based on the current page
            return await _context.Carts
                .Skip((pageNumber - 1) * pageSize) // Skip the items based on page number
                .Take(pageSize) // Take the specified number of items for the current page
                .ToListAsync(); // Execute the query asynchronously and return the result as a list
        }

        // Add an item to the cart
        public async Task<CartItem> AddItemToCartAsync(int cartId, CartItem cartItem)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart == null)
            {
                throw new KeyNotFoundException($"Cart with ID {cartId} not found.");
            }

            cart.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return cartItem;
        }

        // Assuming CartID is the primary key
        public async Task<CartItem> RemoveItemFromCartAsync(int cartId, int cartItemId)
        {
            // Include CartItems to navigate through related entities
            var cart = await _context.Carts.Include(c => c.CartItems)
                                           .FirstOrDefaultAsync(c => c.CartID == cartId); // Use CartID here
            if (cart == null)
            {
                throw new KeyNotFoundException($"Cart with ID {cartId} not found.");
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.CartItemID == cartItemId); // Assuming CartItem has CartItemID
            if (cartItem == null)
            {
                throw new KeyNotFoundException($"CartItem with ID {cartItemId} not found.");
            }

            cart.CartItems.Remove(cartItem); // Remove the item from the cart
            await _context.SaveChangesAsync(); // Save the changes to the database

            return cartItem; // Return the removed item
        }

    }
}
