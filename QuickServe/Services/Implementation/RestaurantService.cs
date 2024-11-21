

using QuickServe.Models;
using QuickServe.Repositories.Interfaces;
using QuickServe.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServe.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;

        // Constructor for dependency injection
        public RestaurantService(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
        {
            // Retrieve restaurant by ID
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null)
            {
                throw new Exception("Restaurant not found.");
            }
            return restaurant;
        }

        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            // Retrieve all restaurants
            return await _restaurantRepository.GetAllRestaurantsAsync();
        }

        public async Task<Restaurant> AddRestaurantAsync(Restaurant restaurant)
        {
            // Validate and add the restaurant
            if (restaurant == null)
            {
                throw new ArgumentNullException(nameof(restaurant), "Restaurant cannot be null.");
            }

            return await _restaurantRepository.AddRestaurantAsync(restaurant);
        }

        public async Task<Restaurant> UpdateRestaurantAsync(Restaurant restaurant)
        {
            // Validate and update restaurant details
            if (restaurant == null)
            {
                throw new ArgumentNullException(nameof(restaurant), "Restaurant cannot be null.");
            }

            var existingRestaurant = await _restaurantRepository.GetRestaurantByIdAsync(restaurant.RestaurantID);
            if (existingRestaurant == null)
            {
                throw new Exception("Restaurant not found.");
            }

            return await _restaurantRepository.UpdateRestaurantAsync(restaurant);
        }

        public async Task<bool> DeleteRestaurantAsync(int id)
        {
            // Delete restaurant by ID
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null)
            {
                throw new Exception("Restaurant not found.");
            }

            return await _restaurantRepository.DeleteRestaurantAsync(id);
        }

        public async Task<IEnumerable<Restaurant>> GetRestaurantsByLocationAsync(string location)
        {
            // Retrieve restaurants by location
            if (string.IsNullOrEmpty(location))
            {
                throw new ArgumentNullException(nameof(location), "Location cannot be empty.");
            }

            return await _restaurantRepository.GetRestaurantsByLocationAsync(location);
        }
    }
}
