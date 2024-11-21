using QuickServe.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServe.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserReadDTO> GetUserByIdAsync(int id);
        Task<IEnumerable<UserReadDTO>> GetAllUsersAsync();
        Task<UserReadDTO> AddUserAsync(UserCreateDTO userDTO);
        Task<UserReadDTO> UpdateUserAsync(int id, UserUpdateDTO userDTO);
        Task<UserReadDTO> LoginUserAsync(UserLoginDTO loginDTO);
        Task<bool> DeleteUserAsync(int id);
    }
}
