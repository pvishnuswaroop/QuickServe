using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuickServe.Data;
using QuickServe.Models;
using QuickServe.Repositories.Interfaces;

namespace QuickServe.Repositories.Implementations
{
    public class RatingRepository : IRatingRepository
    {
        private readonly AppDbContext _context;

        public RatingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Rating> GetRatingByIdAsync(int id)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null)
            {
                throw new KeyNotFoundException($"Rating with ID {id} not found.");
            }
            return rating;
        }


        public async Task<IEnumerable<Rating>> GetRatingsByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Ratings
                .Where(r => r.RestaurantID == restaurantId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rating>> GetRatingsByUserIdAsync(int userId)
        {
            return await _context.Ratings
                .Where(r => r.UserID == userId)
                .ToListAsync();
        }

        public async Task<Rating> AddRatingAsync(Rating rating)
        {
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<Rating> UpdateRatingAsync(Rating rating)
        {
            _context.Ratings.Update(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<bool> DeleteRatingAsync(int id)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null) return false;

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
