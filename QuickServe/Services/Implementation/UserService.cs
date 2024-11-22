using QuickServe.Models;
using QuickServe.Repositories.Interfaces;
using QuickServe.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickServe.DTO;
using Microsoft.EntityFrameworkCore;

namespace QuickServe.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
            _tokenService = tokenService;
        }

        // Register a new user
        public async Task<UserDto> RegisterUserAsync(string email, string password)
        {
            var user = new User
            {
                Email = email,
                PasswordHash = _passwordHasher.HashPassword(null, password),
                Name = "Default Name", // Ensure this is assigned if Name is required
                ContactNumber = "0000000000", // Temporary placeholder if required
                Address = "Default Address", // Placeholder if required
                CreatedAt = DateTime.Now,
                Role = "Customer"
            };

            // Save the user using repository
            await _userRepository.AddUserAsync(user);

            return new UserDto
            {
                Id = user.UserID,
                Email = user.Email,
                Name = user.Name,
                ContactNumber = user.ContactNumber
            };
        }



        // Login the user (authenticate)
        public async Task<string> LoginUserAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email); // Full User object

            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed)
                return null;

            return await _tokenService.GenerateJwtToken(user);
        }

        // Get user by ID
        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                throw new Exception("User not found");

            // Map User to UserDto
            return new UserDto
            {
                Id = user.UserID,
                Email = user.Email,
                Name = user.Name,
                ContactNumber = user.ContactNumber
            };
        }

        // Get user by email
        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                return null;  // Return null explicitly

            // Map User to UserDto
            return new UserDto
            {
                Id = user.UserID,
                Email = user.Email,
                Name = user.Name,
                ContactNumber = user.ContactNumber
            };
        }

        // Get all users (admin functionality)
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            // Map each User to UserDto
            return users.Select(user => new UserDto
            {
                Id = user.UserID,
                Email = user.Email,
                Name = user.Name,
                ContactNumber = user.ContactNumber
            });
        }

        // Update user information
        public async Task<UserDto> UpdateUserAsync(User user)
        {
            // Update user in the database
            var updatedUser = await _userRepository.UpdateUserAsync(user);

            // Map User to UserDto
            return new UserDto
            {
                Id = updatedUser.UserID,
                Email = updatedUser.Email,
                Name = updatedUser.Name,
                ContactNumber = updatedUser.ContactNumber
            };
        }


        // Delete user
        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }
    }
}
