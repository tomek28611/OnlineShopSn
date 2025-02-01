using OnlineShop.Data;
using System.Security.Claims;

namespace OnlineShop.Areas.Admin.Interfaces
{
    public interface IAdminUserService
    {
        int GetUserIdFromClaims(ClaimsPrincipal user);
        Task<User> GetUserByIdAsync(int userId);
    }
}
