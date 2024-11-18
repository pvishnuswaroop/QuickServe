using QuickServe.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServe.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> AddUserAsync(User user);         
        Task<User> UpdateUserAsync(User user);      
        Task<bool> DeleteUserAsync(int id);         
        Task<User> LoginUserAsync(string email, string password);
    }
}
