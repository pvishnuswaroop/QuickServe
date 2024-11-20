using QuickServe.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServe.Repositories.Interfaces
{
    public interface ICartItemRepository
    {
        Task<CartItem?> GetCartItemByIdAsync(int id);  // Nullable return type to handle missing items
        Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId);  // Get all items in a specific cart
        Task<CartItem> AddCartItemAsync(CartItem cartItem);  // Add a new item to a cart
        Task<CartItem> UpdateCartItemAsync(CartItem cartItem);  // Update an existing cart item
        Task<bool> DeleteCartItemAsync(int id);  // Delete a cart item by ID

        // Optional: Pagination for large carts
        Task<IEnumerable<CartItem>> GetCartItemsPaginatedAsync(int cartId, int pageNumber, int pageSize);  // Paginate cart items

        // Optional: Special update methods for quantity or price if frequently modified
        Task<CartItem> UpdateCartItemQuantityAsync(int cartItemId, int newQuantity);  // Update the quantity of a cart item
        Task<CartItem> UpdateCartItemPriceAsync(int cartItemId, decimal newPrice);  // Update the price of a cart item

        // Optional: Method to clear all items from a cart
        Task<bool> ClearCartAsync(int cartId);  // Clear all items from a cart
    }
}
