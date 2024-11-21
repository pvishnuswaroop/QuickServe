using System;
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

        // Get a Menu by ID
        public async Task<Menu?> GetMenuByIdAsync(int id)
        {
            return await _context.Menus
                .Include(m => m.Restaurant)  // Eager load related Restaurant
                .FirstOrDefaultAsync(m => m.MenuID == id);  // Return null if not found
        }

        // Get all menus
        public async Task<IEnumerable<Menu>> GetAllMenusAsync()
        {
            return await _context.Menus
                .Include(m => m.Restaurant)  // Eager load related Restaurant
                .ToListAsync();
        }

        // Add a new menu
        public async Task<Menu> AddMenuAsync(Menu menu)
        {
            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();
            return menu;
        }

        // Update an existing menu
        public async Task<Menu> UpdateMenuAsync(Menu menu)
        {
            _context.Menus.Update(menu);
            await _context.SaveChangesAsync();
            return menu;
        }

        // Delete a menu by ID
        public async Task<bool> DeleteMenuAsync(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu == null) return false;

            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();
            return true;
        }

        // Get menus by Restaurant ID
        public async Task<IEnumerable<Menu>> GetMenusByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Menus
                .Where(m => m.RestaurantID == restaurantId)
                .Include(m => m.Restaurant)  // Eager load related Restaurant
                .ToListAsync();
        }

        // Get menus by Category
        public async Task<IEnumerable<Menu>> GetMenusByCategoryAsync(string category)
        {
            if (string.IsNullOrWhiteSpace(category)) return Enumerable.Empty<Menu>();

            return await _context.Menus
                .Where(m => m.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .Include(m => m.Restaurant)  // Eager load related Restaurant
                .ToListAsync();
        }

        // Search menus by name
        public async Task<IEnumerable<Menu>> SearchMenusByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Enumerable.Empty<Menu>();

            return await _context.Menus
                .Where(m => m.ItemName.Contains(name, StringComparison.OrdinalIgnoreCase))
                .Include(m => m.Restaurant)  // Eager load related Restaurant
                .ToListAsync();
        }

        // Get menus with pagination and optional sorting
        public async Task<IEnumerable<Menu>> GetMenusPaginatedAsync(int pageNumber, int pageSize, string? sortBy = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number and page size must be greater than zero.");
            }

            var query = _context.Menus.AsQueryable();

            // Apply sorting if specified
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "name" => query.OrderBy(m => m.ItemName),
                    "category" => query.OrderBy(m => m.Category),
                    "price" => query.OrderBy(m => m.Price),
                    _ => query.OrderBy(m => m.ItemName) // Default sort by name
                };
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
