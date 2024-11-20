using System.Collections.Generic;
using System.Threading.Tasks;
using QuickServe.Models;

namespace QuickServe.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task<Rating?> GetRatingByIdAsync(int id);  // Use nullable return to indicate the rating may not exist
        Task<IEnumerable<Rating>> GetRatingsByRestaurantIdAsync(int restaurantId);
        Task<IEnumerable<Rating>> GetRatingsByUserIdAsync(int userId);
        Task<IEnumerable<Rating>> GetRatingsByScoreAsync(int score);  // Optional: Get ratings by score
        Task<IEnumerable<Rating>> GetRatingsByDateAsync(DateTime startDate, DateTime endDate);  // Optional: Get ratings within a date range
        Task<Rating> AddRatingAsync(Rating rating);
        Task<Rating> UpdateRatingAsync(Rating rating);
        Task<bool> DeleteRatingAsync(int id);
    }
}
