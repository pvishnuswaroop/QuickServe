using System.Collections.Generic;
using System.Threading.Tasks;
using QuickServe.Models;

namespace QuickServe.Repositories.Interfaces
{
    public interface IRestaurantRepository
    {
        Task<Restaurant?> GetRestaurantByIdAsync(int id);  // Nullable return to handle non-existent restaurants
        Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
        Task<Restaurant> AddRestaurantAsync(Restaurant restaurant);
        Task<Restaurant> UpdateRestaurantAsync(Restaurant restaurant);
        Task<bool> DeleteRestaurantAsync(int id);  // Returns true if deletion was successful, false otherwise

        // Optional: Additional methods for filtering/searching (e.g., by location or cuisine)
        Task<IEnumerable<Restaurant>> GetRestaurantsByLocationAsync(string location);  // Example of a possible additional method
    }
}
