using System.Collections.Generic;
using System.Threading.Tasks;
using QuickServe.Models;

namespace QuickServe.Repositories.Interfaces
{
    public interface IRestaurantRepository
    {
        // CRUD Operations
        Task<Restaurant> GetRestaurantByIdAsync(int id);
        Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
        Task<Restaurant> AddRestaurantAsync(Restaurant restaurant);
        Task<Restaurant> UpdateRestaurantAsync(Restaurant restaurant);
        Task<bool> DeleteRestaurantAsync(int id);

    }
}
