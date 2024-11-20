using System.Collections.Generic;
using System.Threading.Tasks;
using QuickServe.Models;

namespace QuickServe.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(int userId); // Return nullable to handle case when cart doesn't exist for user
        Task<IEnumerable<Cart>> GetAllCartsAsync(); // Retrieve all carts in the system
        Task<Cart> AddCartAsync(Cart cart); // Add a new cart
        Task<Cart> UpdateCartAsync(Cart cart); // Update an existing cart
        Task<bool> DeleteCartAsync(int id); // Delete a cart by ID

        // Optional: Pagination method for large datasets
        Task<IEnumerable<Cart>> GetCartsPaginatedAsync(int pageNumber, int pageSize); // Paginate carts

        // Optional: Managing CartItems (if applicable)
        Task<CartItem> AddItemToCartAsync(int cartId, CartItem cartItem); // Add an item to a cart
        Task<CartItem> RemoveItemFromCartAsync(int cartId, int cartItemId); // Remove an item from the cart
    }
}
