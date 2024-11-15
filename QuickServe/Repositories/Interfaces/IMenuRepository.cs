using System.Collections.Generic;
using System.Threading.Tasks;
using QuickServe.Models;

namespace QuickServe.Repositories.Interfaces
{
    public interface IMenuRepository
    {
        Task<Menu> GetMenuByIdAsync(int id);
        Task<IEnumerable<Menu>> GetAllMenusAsync();
        Task<Menu> AddMenuAsync(Menu menu);
        Task<Menu> UpdateMenuAsync(Menu menu);
        Task<bool> DeleteMenuAsync(int id);
    }
}
