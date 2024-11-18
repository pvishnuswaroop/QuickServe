using System.Collections.Generic;
using System.Threading.Tasks;
using QuickServe.Models;

namespace QuickServe.Repositories.Interfaces
{
    public interface IMenuRepository
    {
        Task<Menu> GetMenuByIdAsync(int id);
        Task<IEnumerable<Menu>> GetAllMenusAsync();
        Task<Menu> AddMenuAsync(Menu menu);
        Task<Menu> UpdateMenuAsync(Menu menu);
        Task<bool> DeleteMenuAsync(int id);

        // Additional functionalities
        Task<IEnumerable<Menu>> GetMenusByRestaurantIdAsync(int restaurantId);  // Get all menus for a specific restaurant
        Task<IEnumerable<Menu>> GetMenusByCategoryAsync(string category);  // Search menus by category
        Task<IEnumerable<Menu>> SearchMenusByNameAsync(string name);  // Search menus by name
        Task<IEnumerable<Menu>> GetMenusPaginatedAsync(int pageNumber, int pageSize);  // Paginate menus
    }
}
