using QuickServe.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServe.Repositories.Interfaces
{
    public interface ICartItemRepository
    {
        Task<CartItem> GetCartItemByIdAsync(int id);
        Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId);  // Get all items in a specific cart
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task<CartItem> UpdateCartItemAsync(CartItem cartItem);
        Task<bool> DeleteCartItemAsync(int id);
    }
}

