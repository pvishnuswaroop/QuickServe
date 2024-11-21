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

        Task<IEnumerable<Menu>> GetMenusByRestaurantIdAsync(int restaurantId);
        Task<IEnumerable<Menu>> GetMenusByCategoryAsync(string category);
        Task<IEnumerable<Menu>> SearchMenusByNameAsync(string name);

        // Pagination with sorting option (optional)
        Task<IEnumerable<Menu>> GetMenusPaginatedAsync(int pageNumber, int pageSize, string? sortBy = null);
    }
}
