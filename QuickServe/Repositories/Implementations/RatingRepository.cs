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
    public class RatingRepository : IRatingRepository
    {
        private readonly AppDbContext _context;

        // Constructor injects the AppDbContext
        public RatingRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get a rating by its ID
        public async Task<Rating?> GetRatingByIdAsync(int id)
        {
            return await _context.Ratings.FindAsync(id);
        }

        // Get all ratings for a specific restaurant
        public async Task<IEnumerable<Rating>> GetRatingsByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Ratings
                .Where(r => r.RestaurantID == restaurantId)
                .ToListAsync();
        }

        // Get all ratings for a specific user
        public async Task<IEnumerable<Rating>> GetRatingsByUserIdAsync(int userId)
        {
            return await _context.Ratings
                .Where(r => r.UserID == userId)
                .ToListAsync();
        }

        // Get ratings by a specific score
        public async Task<IEnumerable<Rating>> GetRatingsByScoreAsync(int score)
        {
            return await _context.Ratings
                .Where(r => r.RatingScore == score)  // Corrected to use RatingScore
                .ToListAsync();
        }

        // Get ratings within a specific date range
        public async Task<IEnumerable<Rating>> GetRatingsByDateAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Ratings
                .Where(r => r.RatingDate >= startDate && r.RatingDate <= endDate)
                .ToListAsync();
        }

        // Add a new rating
        public async Task<Rating> AddRatingAsync(Rating rating)
        {
            // Check if the user has already rated the restaurant
            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserID == rating.UserID && r.RestaurantID == rating.RestaurantID);

            if (existingRating != null)
            {
                throw new InvalidOperationException("User has already rated this restaurant.");
            }

            // Ensure the rating score is valid
            if (rating.RatingScore < 1 || rating.RatingScore > 5)
            {
                throw new InvalidOperationException("Rating score must be between 1 and 5.");
            }

            // Add the rating to the context and save changes
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        // Update an existing rating
        public async Task<Rating> UpdateRatingAsync(Rating rating)
        {
            var existingRating = await _context.Ratings.FindAsync(rating.RatingID);
            if (existingRating == null)
            {
                throw new KeyNotFoundException($"Rating with ID {rating.RatingID} not found.");
            }

            // Ensure the rating score is valid
            if (rating.RatingScore < 1 || rating.RatingScore > 5)
            {
                throw new InvalidOperationException("Rating score must be between 1 and 5.");
            }

            // Update the rating properties
            existingRating.RatingScore = rating.RatingScore;
            existingRating.ReviewText = rating.ReviewText;  // Update other fields as needed

            // Save the changes
            _context.Ratings.Update(existingRating);
            await _context.SaveChangesAsync();
            return existingRating;
        }

        // Delete a rating by its ID
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
