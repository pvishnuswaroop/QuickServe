
using QuickServe.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServe.Services.Interfaces
{
    public interface IRestaurantService
    {
        Task<Restaurant> GetRestaurantByIdAsync(int id);
        Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
        Task<Restaurant> AddRestaurantAsync(Restaurant restaurant);
        Task<Restaurant> UpdateRestaurantAsync(Restaurant restaurant);
        Task<bool> DeleteRestaurantAsync(int id);
        Task<IEnumerable<Restaurant>> GetRestaurantsByLocationAsync(string location);
    }
}
