using Microsoft.EntityFrameworkCore;
using OnlineShop.Areas.Admin.Interfaces;
using OnlineShop.Data;

namespace OnlineShop.Areas.Admin.Services
{
    public class MenusService : IMenusService
    {
        private readonly OnlineShopContext _context;

        public MenusService(OnlineShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Menus>> GetAllMenusAsync()
        {
            return await _context.Menus.ToListAsync();
        }

        public async Task<Menus?> GetMenuDetailsAsync(int? id)
        {
            if (id == null)
                return null;

            return await _context.Menus.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> CreateMenuAsync(Menus menus)
        {
            if (menus == null)
                return false;

            _context.Add(menus);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Menus?> GetMenuForEditAsync(int? id)
        {
            if (id == null)
                return null;

            return await _context.Menus.FindAsync(id);
        }

        public async Task<bool> UpdateMenuAsync(int id, Menus menus)
        {
            if (id != menus.Id)
                return false;

            try
            {
                _context.Update(menus);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuExists(menus.Id))
                    return false;

                throw;
            }
        }

        public async Task<Menus?> GetMenuForDeleteAsync(int? id)
        {
            if (id == null)
                return null;

            return await _context.Menus.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> DeleteMenuAsync(int id)
        {
            var menus = await _context.Menus.FindAsync(id);
            if (menus == null)
                return false;

            _context.Menus.Remove(menus);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.Id == id);
        }
    }
}
