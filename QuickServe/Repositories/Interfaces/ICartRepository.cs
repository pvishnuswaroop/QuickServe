using System.Collections.Generic;
using System.Threading.Tasks;
using QuickServe.Models;

namespace QuickServe.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserIdAsync(int userId); // Retrieve cart by user
        Task<IEnumerable<Cart>> GetAllCartsAsync(); // Retrieve all carts in the system
        Task<Cart> AddCartAsync(Cart cart); // Add a new cart
        Task<Cart> UpdateCartAsync(Cart cart); // Update an existing cart
        Task<bool> DeleteCartAsync(int id); // Delete a cart by ID

    }
}
