using QuickServe.Models;
using QuickServe.DTOs;
using QuickServe.Repositories.Interfaces;
using QuickServe.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace QuickServe.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Get all users (returns IEnumerable of UserReadDTO)
        public async Task<IEnumerable<UserReadDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(MapToUserReadDTO).ToList();
        }

        // Get a user by ID (returns UserReadDTO)
        public async Task<UserReadDTO> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return MapToUserReadDTO(user);
        }

        // Add a new user (returns UserReadDTO)
        public async Task<UserReadDTO> AddUserAsync(UserCreateDTO userDTO)
        {
            var user = new User
            {
                Name = userDTO.Name,
                Gender = userDTO.Gender,
                ContactNumber = userDTO.ContactNumber,
                Email = userDTO.Email,
                Address = userDTO.Address,
                Password = userDTO.Password, // Store the password directly (plaintext)
                DateOfBirth = userDTO.DateOfBirth,
                Role = userDTO.Role
            };

            var createdUser = await _userRepository.AddUserAsync(user);
            return MapToUserReadDTO(createdUser);
        }

        // Update user details (returns UserReadDTO)
        public async Task<UserReadDTO> UpdateUserAsync(int id, UserUpdateDTO userDTO)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) throw new KeyNotFoundException("User not found.");

            user.Name = userDTO.Name ?? user.Name;
            user.Gender = userDTO.Gender ?? user.Gender;
            user.ContactNumber = userDTO.ContactNumber ?? user.ContactNumber;
            user.Email = userDTO.Email ?? user.Email;
            user.Address = userDTO.Address ?? user.Address;

            if (!string.IsNullOrEmpty(userDTO.Password))
            {
                user.Password = userDTO.Password;  // Update plaintext password directly
            }

            var updatedUser = await _userRepository.UpdateUserAsync(user);
            return MapToUserReadDTO(updatedUser);
        }

        // Login user (returns UserReadDTO)
        public async Task<UserReadDTO> LoginUserAsync(UserLoginDTO loginDTO)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDTO.Email);

            if (user == null || !user.ValidatePassword(loginDTO.Password))
            {
                throw new AuthenticationException("Invalid email or password.");
            }

            return MapToUserReadDTO(user);
        }

        // Delete a user (returns boolean indicating success)
        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }

        // Helper method to map User to UserReadDTO
        private UserReadDTO MapToUserReadDTO(User user) =>
            new UserReadDTO
            {
                UserID = user.UserID,
                Name = user.Name,
                ContactNumber = user.ContactNumber,
                Email = user.Email,
                Address = user.Address,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
    }
}
