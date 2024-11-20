using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuickServe.Data;
using QuickServe.Models;
using QuickServe.Repositories.Interfaces;

namespace QuickServe.Repositories.Implementations
{
    public class MenuRepository : IMenuRepository
    {
        private readonly AppDbContext _context;

        public MenuRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Menu> GetMenuByIdAsync(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                throw new KeyNotFoundException($"Menu with ID {id} not found.");
            }
            return menu;
        }

        public async Task<IEnumerable<Menu>> GetAllMenusAsync()
        {
            return await _context.Menus.ToListAsync();
        }

        public async Task<Menu> AddMenuAsync(Menu menu)
        {
            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();
            return menu;
        }

        public async Task<Menu> UpdateMenuAsync(Menu menu)
        {
            _context.Menus.Update(menu);
            await _context.SaveChangesAsync();
            return menu;
        }

        public async Task<bool> DeleteMenuAsync(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu == null) return false;

            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();
            return true;
        }

        // Get menus by restaurant ID
        public async Task<IEnumerable<Menu>> GetMenusByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Menus
                .Where(m => m.RestaurantID == restaurantId)
                .ToListAsync();
        }

        // Get menus by category
        public async Task<IEnumerable<Menu>> GetMenusByCategoryAsync(string category)
        {
            var query = _context.Menus.AsQueryable();
            query = ApplyCategoryFilter(query, category);
            return await query.ToListAsync();
        }

        // Search menus by name
        public async Task<IEnumerable<Menu>> SearchMenusByNameAsync(string name)
        {
            var query = _context.Menus.AsQueryable();
            query = ApplyNameFilter(query, name);
            return await query.ToListAsync();
        }

        // Get menus with pagination
        public async Task<IEnumerable<Menu>> GetMenusPaginatedAsync(int pageNumber, int pageSize, string? sortBy = null)
        {
            var query = _context.Menus.AsQueryable();

            // Apply sorting if specified
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "name":
                        query = query.OrderBy(m => m.ItemName);
                        break;
                    case "category":
                        query = query.OrderBy(m => m.Category);
                        break;
                    case "price":
                        query = query.OrderBy(m => m.Price); // Assuming Price is a property
                        break;
                    default:
                        query = query.OrderBy(m => m.ItemName);
                        break;
                }
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // Utility methods for filtering
        private IQueryable<Menu> ApplyCategoryFilter(IQueryable<Menu> query, string? category)
        {
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(m => m.Category != null && m.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }
            return query;
        }

        private IQueryable<Menu> ApplyNameFilter(IQueryable<Menu> query, string? name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.ItemName != null && m.ItemName.Contains(name, StringComparison.OrdinalIgnoreCase));
            }
            return query;
        }
    }
}
