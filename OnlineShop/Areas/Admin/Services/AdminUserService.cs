using Microsoft.EntityFrameworkCore;
using OnlineShop.Areas.Admin.Interfaces;
using OnlineShop.Models.Db;
using System.Security.Claims;

namespace OnlineShop.Areas.Admin.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly OnlineShopContext _context;

        public AdminUserService(OnlineShopContext context)
        {
            _context = context;
        }

        public int GetUserIdFromClaims(ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}