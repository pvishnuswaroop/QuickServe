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

        // Additional functionalities

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
            // Check if category is null or empty
            if (string.IsNullOrEmpty(category))
            {
                // Optionally, return all menus if category is null or empty
                return await _context.Menus.ToListAsync();
            }

            return await _context.Menus
                .Where(m => m.Category != null && m.Category.Equals(category, StringComparison.OrdinalIgnoreCase)) // Ensure Category is not null
                .ToListAsync();
        }

        // Search menus by name
        public async Task<IEnumerable<Menu>> SearchMenusByNameAsync(string name)
        {
            // Check if name is null or empty
            if (string.IsNullOrEmpty(name))
            {
                return new List<Menu>(); // Optionally return an empty list if name is null or empty
            }

            return await _context.Menus
                .Where(m => m.ItemName != null && m.ItemName.Contains(name, StringComparison.OrdinalIgnoreCase)) // Ensure ItemName is not null
                .ToListAsync();
        }


        // Get menus with pagination
        public async Task<IEnumerable<Menu>> GetMenusPaginatedAsync(int pageNumber, int pageSize)
        {
            return await _context.Menus
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
