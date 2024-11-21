using QuickServe.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServe.Services.Interfaces
{
    public interface IUserService
    {
        
        Task<User> RegisterUserAsync(string email, string password);
        Task<string> LoginUserAsync(string email, string password);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
    }
}