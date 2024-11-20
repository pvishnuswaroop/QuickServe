using System.Collections.Generic;
using System.Threading.Tasks;
using QuickServe.Models;

namespace QuickServe.Repositories.Interfaces
{
    public interface IMenuRepository
    {
        Task<Menu?> GetMenuByIdAsync(int id);  // Return nullable to handle case when menu does not exist
        Task<IEnumerable<Menu>> GetAllMenusAsync();
        Task<Menu> AddMenuAsync(Menu menu);
        Task<Menu> UpdateMenuAsync(Menu menu);
        Task<bool> DeleteMenuAsync(int id);

        // Additional functionalities
        Task<IEnumerable<Menu>> GetMenusByRestaurantIdAsync(int restaurantId);  // Get all menus for a specific restaurant
        Task<IEnumerable<Menu>> GetMenusByCategoryAsync(string category);  // Search menus by category
        Task<IEnumerable<Menu>> SearchMenusByNameAsync(string name);  // Search menus by name

        // Pagination with sorting option (optional)
        Task<IEnumerable<Menu>> GetMenusPaginatedAsync(int pageNumber, int pageSize, string? sortBy = null);  // Paginate menus with optional sorting
    }
}
