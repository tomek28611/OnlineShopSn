using OnlineShop.Areas.Admin.Interfaces;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models.Db;

namespace OnlineShop.Areas.Admin.Services
{
    public class UsersService : IUsersService
    {
        private readonly OnlineShopContext _context;
        public UsersService(OnlineShopContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetUserForEditAsync(int? id)
        {
            if (id == null)
                return null;

            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> UpdateUserAsync(int id, User user)
        {
            if (id != user.Id)
                return false;

            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
                    return false;

                throw;
            }
        }

        public async Task<User?> GetUserForDeleteAsync(int? id)
        {
            if (id == null)
                return null;

            return await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }


        public bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
