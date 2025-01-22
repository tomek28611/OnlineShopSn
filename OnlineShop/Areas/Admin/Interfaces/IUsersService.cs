using System.Threading.Tasks;
using OnlineShop.Models.Db;

namespace OnlineShop.Areas.Admin.Interfaces
{
    public interface IUsersService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<bool> CreateUserAsync(User user);
        Task<User?> GetUserForEditAsync(int? id);
        Task<bool> UpdateUserAsync(int id, User user);
        Task<User?> GetUserForDeleteAsync(int? id);
        Task<bool> DeleteUserAsync(int id);
        bool UserExists(int id);

    }
}
