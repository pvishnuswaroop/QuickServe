using QuickServe.DTO;
using QuickServe.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServe.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterUserAsync(string email, string password);
        Task<string> LoginUserAsync(string email, string password);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
    }
}
