using System.Collections.Generic;
using System.Threading.Tasks;
using QuickServe.Models;

namespace QuickServe.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task<Rating> GetRatingByIdAsync(int id);
        Task<IEnumerable<Rating>> GetRatingsByRestaurantIdAsync(int restaurantId);
        Task<IEnumerable<Rating>> GetRatingsByUserIdAsync(int userId);
        Task<Rating> AddRatingAsync(Rating rating);
        Task<Rating> UpdateRatingAsync(Rating rating);
        Task<bool> DeleteRatingAsync(int id);
    }
}
